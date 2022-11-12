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

        sm = fsm as PlayerStateMachine;

        DeathEventChannel.RaiseEvent();
        sm.Freeze();
    }
}
