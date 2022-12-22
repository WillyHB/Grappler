using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/EmoteState")]
public class PlayerEmoteState : State
{
    public string Animation { get; set; }

    private PlayerStateMachine sm;
    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm = fsm as PlayerStateMachine;

        sm.Animator.Play(Animation);
    }

    public override void Update()
    {
        base.Update();

        if (sm.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !sm.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.isLooping)
        {
            Debug.Log(sm.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.isLooping);
            Debug.Log(sm.Animator.GetCurrentAnimatorClipInfo(0)[0].clip);
            Debug.Log("OH SHIT");
            sm.Transition(sm.IdleState);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
