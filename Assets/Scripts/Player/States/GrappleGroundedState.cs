using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleGroundedState : GroundedState
{
    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
    }

    public override void Update()
    {
        base.Update();

        if (!sm.IsGrounded)
        {
            sm.Transition(sm.GrappleState);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            sm.Transition(sm.JumpState);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            FindObjectOfType<Grapple>().ReleaseGrapple();

            sm.Transition(sm.IdleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
