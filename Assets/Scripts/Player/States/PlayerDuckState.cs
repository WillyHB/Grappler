using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Duck")]
public class PlayerDuckState : GroundedState
{
    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm.Animator.Play(sm.Animations.Duck);
    }

    public override void Update()
    {
        base.Update();

        if (!sm.InputProvider.GetState().IsCrouching)
        {
            sm.Transition(sm.IdleState);
            return;
        }
    }
}
