using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="States/Player/JumpState")]
public class PlayerJumpState : State
{
    PlayerStateMachine sm;

    public float JumpForce = 200;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm = fsm as PlayerStateMachine;

        sm.Rigidbody.velocity = new Vector2(sm.Rigidbody.velocity.x, sm.Rigidbody.velocity.y < 0 ? 0 : sm.Rigidbody.velocity.y);
        sm.Rigidbody.AddForce(Vector2.up * JumpForce);

        
        sm.Transition(sm.FallState);
    }
}
