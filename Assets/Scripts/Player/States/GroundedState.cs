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
        base.OnEnter(fsm);

        sm = (PlayerStateMachine)fsm;
        //accelerant = sm.Rigidbody.velocity.x;
        accelerant = sm.Velocity.x;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        sm.Velocity = new Vector2(accelerant, sm.Velocity.y);
        //sm.Rigidbody.velocity = new Vector2(accelerant, sm.Rigidbody.velocity.y);

    }

    public override void Update()
    {
        base.Update();


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
