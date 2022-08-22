using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/IdleState")]
public class PlayerIdleState : GroundedState
{
    public float GroundFriction = 25;

    public float TimeBetweenAnimations = 10;

    private float animationTimer;

    private int prevAnim;


    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        animationTimer = 0;
        sm.Animator.Play(sm.Animations.Idle);
    }

    public override void Update()
    {
        base.Update();

        if (sm.MoveValue != 0)
        {
            if (InputDeviceManager.CurrentDeviceType == InputDevices.MnK && sm.InputProvider.GetState().IsWalking)
            {
                sm.Transition(sm.WalkState);
                return;
            }

            sm.Transition(sm.RunState);
            return;
        }

        animationTimer += Time.deltaTime;

        if (animationTimer >= TimeBetweenAnimations)
        {
            if (sm.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == sm.Animations.Idle)
            {
                int anim = prevAnim;

                while (anim == prevAnim)
                {
                    anim = Random.Range(1, 5);
                }

                sm.Animator.Play($"IdleAnimation4");
                animationTimer = 0;

                prevAnim = anim;
            }
        }
    }
}
