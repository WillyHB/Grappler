using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/CrouchIdle")]
public class PlayerCrouchIdle : GroundedState
{
    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm.Animator.Play("CrouchIdle");
    }

    public override void Update()
    {
        base.Update();
        
        if (!sm.InputProvider.GetState().IsCrouching)
        {
            sm.Transition(sm.IdleState);
            return;
        }

        if (sm.MoveValue != 0)
        {
            sm.Transition(sm.CrouchMove);
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
