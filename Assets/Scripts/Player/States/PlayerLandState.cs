using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(menuName ="States/Player/LandState")]
public class PlayerLandState : PlayerMoveState
{
    public RumbleEventChannel rumbleChannel;
    public GameObject LandDust;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
        rumbleChannel.PerformRumble(1f, 0.25f, 0.2f);
        Instantiate(LandDust, sm.transform.position, Quaternion.identity);
        sm.Animator.Play(sm.Animations.Land);
        Audio land = sm.GroundSoundManager.GetCurrentTileSounds()?.Land;

        if (land != null) sm.PlayerAudioEventChannel.Play(land);
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
            if (sm.InputProvider.GetState().IsCrouching)
            {
                sm.Transition(sm.Duck);
                return;
            }

            if (sm.MoveValue != 0)
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
