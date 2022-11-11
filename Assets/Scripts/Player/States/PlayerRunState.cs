using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/RunState")]
public class PlayerRunState : PlayerMoveState
{
    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm.Animator.Play(sm.Animations.Run);
    }

    public override void Update()
    {
        base.Update();

        if (sm.InputProvider.GetState().IsCrouching)
        {
            sm.Transition(sm.Duck);
            return;
        }

        if (sm.MoveValue == 0)
        {
            sm.Transition(sm.IdleState);
            return;
        }

        else if (sm.InputProvider.GetState().IsWalking)
        {
            sm.Transition(sm.WalkState);
            return;
        }

        if (sm.Grapple.IsGrappling && sm.Grapple.ConnectionRope.IsStretched())
        {
            sm.Animator.Play(sm.Animations.GrapplePull);
        }

        else
        {
            sm.Animator.Play(sm.Animations.Run);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
