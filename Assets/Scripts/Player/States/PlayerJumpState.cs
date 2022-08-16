using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="States/Player/JumpState")]
public class PlayerJumpState : State
{
    PlayerStateMachine sm;

    public float JumpForce = 200;
    public float MaxJumpTime;
    public float JumpTimeAccelerator = 1;

        private float jumpTime;
    private float defaultGravity;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm = fsm as PlayerStateMachine;

        jumpTime = 0;

        defaultGravity = sm.Rigidbody.gravityScale;

        sm.Rigidbody.velocity = new Vector2(sm.Rigidbody.velocity.x, sm.Rigidbody.velocity.y < 0 ? 0 : sm.Rigidbody.velocity.y);
        sm.Rigidbody.AddForce(Vector2.up * JumpForce);
    }

    public override void Update()
    {
        
        jumpTime += JumpTimeAccelerator * Time.deltaTime;


        if (sm.InputProvider.GetState().IsJumping && jumpTime < MaxJumpTime)
        {
            sm.Rigidbody.gravityScale = defaultGravity / 2;

        }

        else
        {
            sm.Rigidbody.gravityScale = defaultGravity;
            sm.Transition(sm.FallState);
        }
    }
}
