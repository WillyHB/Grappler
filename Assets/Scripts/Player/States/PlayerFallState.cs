using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElRaccoone.Tweens;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName ="States/Player/FallState")]
public class PlayerFallState : State
{
    PlayerStateMachine sm;

    private bool jumpBuffered;

    public float JumpBufferTime = 1;
    private float timer;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm = fsm as PlayerStateMachine;

        sm.PlayerInput.actions["Jump"].started += Jump;
    }

    public override void OnExit()
    {
        base.OnExit();

        sm.PlayerInput.actions["Jump"].started -= Jump;
    }

    private void Jump(InputAction.CallbackContext cc)
    {
        jumpBuffered = true;
        timer = 0;
    }

    public override void Update()
    {
        base.Update();

        if (timer > JumpBufferTime)
        {
            jumpBuffered = false;
        }

        if (sm.IsGrounded)
        {
            if (sm.Rigidbody.velocity.y < 0)
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

        if (jumpBuffered)
        {
            timer += 1 * Time.deltaTime;
        }

    }
}
