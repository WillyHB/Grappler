using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/DeathState")]
public class PlayerDeathState : State
{
    public VoidEventChannel DeathEventChannel;
    PlayerStateMachine sm;
    public override void OnEnter(StateMachine fsm)
    {
        Debug.Log("here I guess");
        base.OnEnter(fsm);
        sm = fsm as PlayerStateMachine;

        sm.Freeze(false);
        sm.Animator.Play(sm.Animations.Death);
    }

    public override void Update()
    {
        base.Update();

        if (sm.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >=1) 
        {
            LevelTransition.Reload();
        }
    }

}
