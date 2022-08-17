using System;
using UnityEngine;
using ElRaccoone.Tweens;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName ="States/Player/FallState")]
public class PlayerFallState : PlayerAirborneState
{
    private bool jumpBuffered;

    public float JumpBufferTime = 1;
    private float jumpTimer;

    private bool coyoteTimeEnabled = false;
    public float CoyoteTime = 0.1f;
    private float coyoteTimer;

    public float MaxFallSpeed = 100;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm.InputProvider.Jumped += Jump;

        coyoteTimeEnabled = sm.PreviousState.IsSubclassOf(typeof(GroundedState));
        coyoteTimer = 0;
    }

    public override void OnExit()
    {

        base.OnExit();

        sm.InputProvider.Jumped -= Jump;
    }

    private void Jump()
    {
        if (coyoteTimeEnabled)
        {
            if (coyoteTimer < CoyoteTime)
            {
                sm.Transition(sm.JumpState);
                return;
            }
        }

        jumpBuffered = true;
        jumpTimer = 0;
    }

    public override void Update()
    {
        base.Update();


        if (jumpTimer > JumpBufferTime)
        {
            jumpBuffered = false;
        }

        if (sm.IsGrounded)
        {
            if (jumpBuffered)
            {
                sm.Transition(sm.JumpState);
                jumpBuffered = false;
                return;
            }

            sm.Transition(sm.IdleState);
            return;
        }

        if (sm.Grapple.IsGrappling)
        {
            sm.Transition(sm.GrappleState);
        }
    }


    public override void FixedUpdate()
    {
        base.FixedUpdate();


        if (sm.Rigidbody.velocity.y < -MaxFallSpeed)
        {
            sm.Rigidbody.velocity = new Vector2(sm.Rigidbody.velocity.x, -MaxFallSpeed);
        }

        if (jumpBuffered)
        {
            jumpTimer += 1 * Time.deltaTime;
        }

        if (coyoteTimeEnabled)
        {
            coyoteTimer += 1 * Time.deltaTime;
        }
    }
}
