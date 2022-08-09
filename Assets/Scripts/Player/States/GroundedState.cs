using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundedState : State
{

    protected PlayerStateMachine sm;

    protected float accelerant;

    protected float moveValue;



    public override void OnEnter(StateMachine fsm)
    {
        sm = (PlayerStateMachine)fsm;
        accelerant = sm.Rigidbody.velocity.x;
    }

    public override void Update()
    {
        base.Update();

        sm.Rigidbody.velocity = new Vector2(accelerant, sm.Rigidbody.velocity.y);

        moveValue = sm.PlayerInput.actions["Move"].ReadValue<float>();

        if (sm.PlayerInput.actions["Jump"].WasPressedThisFrame())
        {
            sm.Transition(sm.JumpState);
        }

        if (!sm.IsGrounded)
        {
            if (sm.Grapple.IsGrappling)
            {
                sm.Transition(sm.GrappleState);
                return;
            }

            sm.Transition(sm.FallState);
        }
    }
}
