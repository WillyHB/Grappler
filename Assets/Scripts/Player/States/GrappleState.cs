using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ElRaccoone.Tweens;

[CreateAssetMenu(menuName ="States/Player/GrappleState")]
public class GrappleState : State
{
    public float GrapplePower = 50;

    protected PlayerStateMachine sm;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
        sm = fsm as PlayerStateMachine;
    }

    public override void Update()
    {
        base.Update();

        // CHECK GRAPPLE SCRIPT FOR GRAPPLE LENGTH MODIFICATIONS

        float movementVal = sm.PlayerInput.actions["Move"].ReadValue<float>();
        sm.Rigidbody.AddForce(new Vector2(movementVal * (GrapplePower * Time.deltaTime), 0));

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FindObjectOfType<Grapple>().ReleaseGrapple();
            sm.transform.TweenRotation(Vector3.zero, 0.1f);
            sm.Transition(sm.JumpState);

        }

        if (sm.IsGrounded)
        {
            sm.transform.TweenRotation(Vector3.zero, 0.05f);
            sm.Transition(sm.IdleState);
        }
    }

}
