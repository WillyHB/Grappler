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

    public GameObject JumpDust;

    private bool trick = false;
    private int trickID;
    public void PerformTrickNextJump(int trickID) 
    {
        this.trickID = trickID;
        trick = true;
    }

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
        Instantiate(JumpDust, sm.transform.position, Quaternion.identity);

        startY = sm.transform.position.y;
        defaultGravity = sm.Rigidbody.gravityScale;

        sm.Rigidbody.velocity = new Vector2(sm.Rigidbody.velocity.x, sm.Rigidbody.velocity.y < 0 ? 0 : sm.Rigidbody.velocity.y);
        sm.Rigidbody.AddForce(Vector2.up * JumpForce);

        sm.Animator.Play(trick ? trickID : sm.Animations.Jump);
        trick = false;
    }

    public override void OnExit()
    {
        base.OnExit();

        sm.Rigidbody.gravityScale = defaultGravity;
    }

    public override void Update()
    {

        if (sm.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >=1
            && (!sm.InputProvider.GetState().IsJumping
            || sm.transform.position.y - startY >= MaxJumpHeight
            || sm.Rigidbody.velocity.y < 0))
        {
            if (sm.Grapple.IsGrappling)
            {
                sm.Transition(sm.GrappleState);
                return;
            }

            sm.Transition(sm.FallState);
            return;
        }

        else if (!sm.InputProvider.GetState().IsJumping)
        {
            sm.Rigidbody.gravityScale = defaultGravity;
        }

        else
        {
            sm.Rigidbody.gravityScale = JumpGravityModifier;
        }
    }
}
