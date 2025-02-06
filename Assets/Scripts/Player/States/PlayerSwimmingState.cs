using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/SwimState")]
public class PlayerSwimmingState : State
{
    protected PlayerStateMachine sm;

    protected Vector2 accelerant;

    public int VerticalDampening = 200;
    public int HorizontalDampening;

    public int HorizontalAccelerant;
    public int VerticalAccelerant;

    public int HorizontalSpeed;
    public int VerticalSpeed;

    public float FootSurfaceDistance = 0.1f;
    public float MaxSurfaceDistance = 0.1f;

    [Range(0f, 1f)]
    public float SurfaceTension;

    public bool NearSurface;

    private float oldGravity;

    public Audio UnderwaterSoundEffects;
    public Audio EmergeSoundEffect;
    private bool playingUnderwaterSound;


    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        playingUnderwaterSound = false;
        sm = (PlayerStateMachine)fsm;
        accelerant = sm.Rigidbody.velocity;
        oldGravity = sm.Rigidbody.gravityScale;
        sm.Rigidbody.gravityScale = 0;

        sm.Animator.Play(sm.Animations.Swim);
        sm.InputProvider.Jumped += Jump;

        accelerant.y += -accelerant.y * SurfaceTension;
    }

    public override void OnExit()
    {
        base.OnExit();

        sm.InputProvider.Jumped -= Jump;
        if (sm.Rigidbody != null)
        {
            sm.Rigidbody.gravityScale = oldGravity;
        }

        sm.EnvironmentAudioEventChannel.Stop(UnderwaterSoundEffects);
        //AudioMaster.Instance.Stop(UnderwaterSoundEffects);
    }

    private void Jump()
    {
        if (NearSurface)
        {
            sm.CurrentWater = null;
            sm.Transition(sm.JumpState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float speedx = sm.MoveValue * (HorizontalSpeed * Time.deltaTime);
        float speedy = sm.InputProvider.GetState().SwimDirection * (VerticalSpeed * Time.deltaTime);

        if (speedy > 0 && NearSurface)
        {
            speedy = 0;
        }

        if (speedx > 0)
        {
            if (accelerant.x < speedx)
            {
                accelerant.x += HorizontalAccelerant * Time.deltaTime;
            }

            else
            {
                accelerant.x = speedx;
            }
        }

        else if (speedx < 0)
        {
            if (accelerant.x > speedx)
            {
                accelerant.x -= HorizontalAccelerant * Time.deltaTime;
            }

            else
            {
                accelerant.x = speedx;
            }
        }

        else
        {
            if (accelerant.x < 0)
            {
                accelerant.x += Mathf.Min(HorizontalDampening * Time.deltaTime, -accelerant.x);
            }

            else if (accelerant.x > 0)
            {
                accelerant.x -= Mathf.Min(HorizontalDampening * Time.deltaTime, accelerant.x);
            }
        }

        if (speedy > 0)
        {
            if (accelerant.y < speedy)
            {
                accelerant.y += VerticalAccelerant * Time.deltaTime;
            }

            else
            {
                accelerant.y = speedy;
            }


        }

        else if (speedy < 0)
        {
            if (accelerant.y > speedy)
            {
                accelerant.y -= VerticalAccelerant * Time.deltaTime;
            }

            else
            {
                accelerant.y = speedy;
            }
        }

        else
        {
            if (accelerant.y < 0)
            {
                accelerant.y += Mathf.Min(VerticalDampening * Time.deltaTime, -accelerant.y);
            }

            else if (accelerant.y > 0)
            {
                accelerant.y -= Mathf.Min(VerticalDampening * Time.deltaTime, accelerant.y);
            }
        }

        sm.Rigidbody.velocity = accelerant;
    }

    public override void Update()
    {
        base.Update();

        if (sm.MoveValue != 0)
        {
            sm.Animator.Play(sm.Animations.SwimMove);
        }

        else
        {
            sm.Animator.Play(sm.Animations.Swim);
        }

        if (sm.CurrentWater == null)
        {
            sm.Transition(sm.IdleState);
            return;
        }

        NearSurface = sm.transform.position.y - sm.GetComponent<BoxCollider2D>().bounds.size.y / 2
            > sm.CurrentWater.transform.position.y
            + sm.CurrentWater.GetComponent<BoxCollider2D>().bounds.size.y / 2
            + sm.CurrentWater.GetComponent<BoxCollider2D>().offset.y
            - MaxSurfaceDistance;

        if (!NearSurface && !playingUnderwaterSound) 
        {
            playingUnderwaterSound = true;
            sm.EnvironmentAudioEventChannel.Play(UnderwaterSoundEffects);
            //AudioMaster.Instance.Play(UnderwaterSoundEffects, MixerGroup.Environment);
        }

        else if (NearSurface && playingUnderwaterSound) 
        {
            playingUnderwaterSound = false;
            sm.EnvironmentAudioEventChannel.Stop(UnderwaterSoundEffects);
            //AudioMaster.Instance.Stop(UnderwaterSoundEffects);
            sm.EnvironmentAudioEventChannel.Play(EmergeSoundEffect);
            //AudioMaster.Instance.Play(EmergeSoundEffect, MixerGroup.Environment);

        }
    }
}
