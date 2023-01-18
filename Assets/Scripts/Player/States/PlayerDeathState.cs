using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/DeathState")]
public class PlayerDeathState : State
{
    public VoidEventChannel DeathEventChannel;
    PlayerStateMachine sm;
    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
        Debug.LogWarning("DIE!");
        sm = fsm as PlayerStateMachine;

        sm.Freeze();

        sm.Transition(sm.IdleState);

        LevelTransition.Reload();

        //DeathEventChannel.RaiseEvent();

    }
}
