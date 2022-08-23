using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/WalkState")]
public class PlayerWalkState : PlayerMoveState
{
    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm.Animator.Play(sm.Animations.Walk);
    }

    public override void Update()
    {
        base.Update();

        if (sm.MoveValue == 0)
        {
            sm.Transition(sm.IdleState);
            return;
        }

        else if (!sm.InputProvider.GetState().IsWalking)
        {
            sm.Transition(sm.RunState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
