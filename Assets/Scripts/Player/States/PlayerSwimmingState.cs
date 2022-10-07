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

    public bool NearSurface => sm.transform.position.y - sm.GetComponent<CapsuleCollider2D>().bounds.size.y / 2
            > sm.CurrentWater.transform.position.y
            + sm.CurrentWater.GetComponent<BoxCollider2D>().bounds.size.y / 2
            + sm.CurrentWater.GetComponent<BoxCollider2D>().offset.y
            - MaxSurfaceDistance;

    private float oldGravity;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm = (PlayerStateMachine)fsm;
        accelerant = sm.Rigidbody.velocity;
        oldGravity = sm.Rigidbody.gravityScale;
        sm.Rigidbody.gravityScale = 0;

        sm.InputProvider.Jumped += Jump;
    }

    public override void OnExit()
    {
        sm.InputProvider.Jumped -= Jump;
        sm.Rigidbody.gravityScale = oldGravity;
        base.OnExit();
    }

    private void Jump()
    {
        if (NearSurface)
        {
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

        if (sm.CurrentWater == null) sm.Transition(sm.IdleState);
        // if at the waters surface, don't allow further climbing

        // if near surface, push to top

        // allow jumping out when at surface


    }
}
