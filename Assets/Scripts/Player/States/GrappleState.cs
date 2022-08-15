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

        if (!sm.Grapple.IsGrappling)
        {
            sm.Transition(sm.IdleState);

            return;
        }

        ConnectionRope cr = sm.GetComponentInChildren<ConnectionRope>();

        if (cr.GetLength() <= cr.GetDistanceBetweenBodies())
        {
            Vector3 dir = (Vector2)sm.transform.position - cr.GetCalculatedEndPoint();

            Quaternion rot = Quaternion.LookRotation(Vector3.forward, -dir);

            sm.transform.rotation = rot;
        }


        float movementVal = sm.InputProvider.GetState().MoveDirection;
        sm.Rigidbody.AddForce(new Vector2(movementVal * (GrapplePower * Time.deltaTime), 0));

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            sm.Grapple.ReleaseGrapple();
            sm.Transition(sm.JumpState);

        }

        if (sm.IsGrounded)
        {
            sm.Transition(sm.IdleState);
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        sm.transform.rotation = Quaternion.identity;
    }

}
