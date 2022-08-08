using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/IdleState")]
public class PlayerIdleState : State
{

    PlayerStateMachine sm;
    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm = fsm as PlayerStateMachine;
    }

    public override void Update()
    {
        base.Update();

        if (sm.PlayerInput.actions["Move"].ReadValue<float>() != 0)
        {
            sm.Transition(sm.MoveState);
        }
    }
}
