using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="States/Player/JumpState")]
public class PlayerJumpState : PlayerAirborneState
{
    public float JumpForce = 200;
    public float JumpGravityModifier = 0.5f;
    private float startY;
    public float MaxJumpHeight;

    private float defaultGravity;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
        startY = sm.transform.position.y;
        defaultGravity = sm.Rigidbody.gravityScale;

        sm.Rigidbody.velocity = new Vector2(sm.Rigidbody.velocity.x, sm.Rigidbody.velocity.y < 0 ? 0 : sm.Rigidbody.velocity.y);
        sm.Rigidbody.AddForce(Vector2.up * JumpForce);
    }

    public override void Update()
    {
        if (!sm.InputProvider.GetState().IsJumping 
            || sm.transform.position.y - startY >= MaxJumpHeight 
            || sm.Rigidbody.velocity.y < 0)
        {
            sm.Rigidbody.gravityScale = defaultGravity;

            sm.Animator.SetBool("isJumping", false);
            sm.Transition(sm.FallState);
        }

        else
        {
            sm.Rigidbody.gravityScale = JumpGravityModifier;
        }
    }
}
