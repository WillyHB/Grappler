using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="States/Player/LandState")]
public class PlayerLandState : PlayerMoveState
{
    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
        sm.Animator.Play(sm.Animations.Land);
    }

    public override void Update()
    {
        base.Update();

        if(sm.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            sm.Transition(sm.IdleState);
            return;
        }

        else
        {
            if (sm.MoveValue > 0 || sm.MoveValue < 0)
            {
                if (sm.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash != sm.Animations.RunLand)
                    sm.Animator.Play(sm.Animations.RunLand, 0, sm.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }

            else
            {
                if (sm.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash != sm.Animations.Land)
                    sm.Animator.Play(sm.Animations.Land, 0, sm.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
