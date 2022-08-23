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

        if (sm.MoveValue == 0)
        {
            if (accelerant < 0)
            {
                accelerant += Mathf.Min(40 * Time.deltaTime, -accelerant);
            }

            else if (accelerant > 0)
            {
                accelerant -= Mathf.Min(40 * Time.deltaTime, accelerant);
            }
        }

        sm.Rigidbody.velocity = new Vector2(accelerant, sm.Rigidbody.velocity.y);

    }

    public override void Update()
    {
        base.Update();

        if (!sm.IsGrounded)
        {
            if (sm.Grapple.IsGrappling)
            {
                sm.Transition(sm.GrappleState);
                return;
            }

            sm.Transition(sm.FallState);
            return;
        }
    }
}
