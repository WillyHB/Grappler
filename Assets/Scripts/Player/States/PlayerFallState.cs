using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElRaccoone.Tweens;

[CreateAssetMenu(menuName ="States/Player/FallState")]
public class PlayerFallState : State
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

        if (sm.IsGrounded)
        {
            if (sm.Rigidbody.velocity.y < 0)
            {
                sm.Transition(sm.IdleState);
            }      
        }

        if (FindObjectOfType<Grapple>().IsGrappling)
        {
            sm.Transition(sm.GrappleState);
        }

        /*
        float value = sm.PlayerInput.actions["Move"].ReadValue<float>();

        sm.Rigidbody.AddForce(200 * Time.deltaTime * value * Vector2.right);*/
    }


    public override void FixedUpdate()
    {
        base.FixedUpdate();



    }
}
