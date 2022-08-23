using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/CrouchMove")]
public class PlayerCrouchMoveState : PlayerMoveState
{
    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
    }
    public override void Update()
    {
        base.Update();

        if (!sm.InputProvider.GetState().IsCrouching)
        {
            sm.Transition(sm.RunState);
            return;
        }

        if (sm.MoveValue == 0)
        {
            sm.Animator.Play("CrouchIdle");
            sm.Transition(sm.CrouchIdle);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
