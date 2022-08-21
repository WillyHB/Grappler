using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundedState : State
{

    protected PlayerStateMachine sm;

    protected float accelerant;


    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm = (PlayerStateMachine)fsm;
        accelerant = sm.Rigidbody.velocity.x;
        sm.InputProvider.Jumped += Jump;
    }

    private void Jump()
    {
        sm.Animator.SetBool("isJumping", true);
        sm.Animator.Play("Jump");
        sm.Transition(sm.JumpState);
    }

    public override void OnExit()
    {
        base.OnExit();


        sm.InputProvider.Jumped -= Jump;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        sm.Rigidbody.velocity = new Vector2(accelerant, sm.Rigidbody.velocity.y);

    }

    public override void Update()
    {
        base.Update();

        if (!sm.Animator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
        sm.Animator.Play(sm.MoveValue > 0 || sm.MoveValue < 0
? (InputDeviceManager.CurrentDeviceType == InputDevices.MnK || sm.InputProvider.GetState().IsWalking ? "Walk" : "Run")
: "Idle");

        if (!sm.IsGrounded)
        {
            if (sm.Grapple.IsGrappling)
            {
                sm.Animator.Play("Grapple");
                sm.Transition(sm.GrappleState);
                return;
            }

            sm.Animator.Play("FallDown");
            sm.Transition(sm.FallState);
            return;
        }
    }
}
