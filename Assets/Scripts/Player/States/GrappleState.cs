using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName ="States/Player/GrappleState")]
public class GrappleState : State
{
    public float GrapplePower = 50;

    public float RotationDistanceLimit = 0.05f;

    private bool isJumping;
    protected PlayerStateMachine sm;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
        sm = fsm as PlayerStateMachine;

        sm.InputProvider.Jumped += Jump;

        sm.Animator.Play(sm.Animations.Grapple);
        isJumping = false;
    }

    private void Jump()
    {
        isJumping = true;
        sm.Transition(sm.GrappleExitState);
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

        if (Mathf.Abs(cr.GetLength() - cr.GetDistanceBetweenBodies()) < RotationDistanceLimit)
        {
            Vector3 dir = (Vector2)sm.transform.position - cr.GetCalculatedEndPoint();

            Quaternion rot = Quaternion.LookRotation(Vector3.forward, -dir);

            sm.transform.rotation = rot;
        }


        float movementVal = sm.InputProvider.GetState().MoveDirection;
        sm.Rigidbody.AddForce(new Vector2(movementVal * (GrapplePower * Time.deltaTime), 0));

        if (sm.IsGrounded)
        {
            sm.Transition(sm.LandState);
            return;
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        sm.Grapple.ReleaseGrapple();

        if (!isJumping)
        sm.transform.rotation = Quaternion.identity;

        sm.InputProvider.Jumped -= Jump;
    }

}
