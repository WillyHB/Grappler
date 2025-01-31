using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(menuName ="States/Player/GrappleExit")]
public class GrappleExitState : PlayerAirborneState
{
    public float JumpForce = 300;

    private int animation = -1;
    public void ForceSetNextTrick(int animation) 
    {
        this.animation = animation;
    }

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        float rot = sm.transform.eulerAngles.z % 360;

        sm.transform.rotation = Quaternion.identity;
        
        if (sm.GetComponent<SpriteRenderer>().flipX) rot = 360 - rot;

        if (animation != -1) 
        {
            sm.Animator.Play(animation);
            animation = -1;
        }
        else 
        {
            Debug.Log(rot);
            sm.Animator.Play(rot switch
            {
                >= 350 => sm.Animations.GrappleSpinExit,
                >= 320 => sm.Animations.GrappleTuckedSpinExit,
                >= 240 => sm.Animations.GrappleFrontFlipExit,
                >= 135 => sm.Animations.GrappleUDBackFlipExit,
                >= 40 => sm.Animations.GrappleBackFlipExit,
                >= 30 => sm.Animations.GrappleTuckedSpinExit,
                >= 15 => sm.Animations.GrappleJumpExit,
                >= 0 => sm.Animations.GrappleSpinExit,
                _ => throw new System.Exception($"Angle {rot} does not exist wtf"),

            });
        }

        if(sm.Rigidbody.velocity.y < 0)
        {
            sm.Rigidbody.AddForce(Vector2.up * JumpForce + sm.Rigidbody.velocity);
        }

        else
        {
            sm.Rigidbody.AddForce(Vector2.up * JumpForce);
        }

    }

    public override void Update()
    {
        base.Update();

        if (sm.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
                && !sm.Animator.GetCurrentAnimatorStateInfo(0).IsName("Grapple"))
        {
            sm.Transition(sm.FallState);
            return;
        }

        if (sm.IsGrounded)
        {
            sm.Transition(sm.LandState);
            return;
        }

        if (sm.Grapple.IsGrappling)
        {
            sm.Transition(sm.GrappleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
