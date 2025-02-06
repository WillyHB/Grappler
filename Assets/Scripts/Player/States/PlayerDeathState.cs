using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/DeathState")]
public class PlayerDeathState : State
{
    public VoidEventChannel DeathEventChannel;
    public Audio DeathSound;
    PlayerStateMachine sm;
    private bool completelyDead;
    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
        completelyDead = false;
        sm = fsm as PlayerStateMachine;

        AudioMaster.Instance.Play(DeathSound, MixerGroup.Player);

        sm.Freeze(false);
        sm.Animator.Play(sm.Animations.Death);
    }

    public override void Update()
    {
        base.Update();

        if (sm.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >=1 && !completelyDead) 
        {
            //DO SOME SCREEN EFFECT
            completelyDead = true;

            LevelTransition.Reload();
        }
    }

}
