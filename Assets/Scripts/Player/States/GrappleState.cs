using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ElRaccoone.Tweens;

[CreateAssetMenu(menuName ="States/Player/GrappleState")]
public class GrappleState : State
{
    public float GrapplePower = 50;

    public float GrappleExtensionSpeed = 2;
    private PlayerStateMachine sm;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
        sm = fsm as PlayerStateMachine;
    }

    public override void Update()
    {
        Vector3 dir = sm.transform.position - GameObject.Find("Hook").transform.position;

        Quaternion rot = Quaternion.LookRotation(Vector3.forward, -dir);

        sm.transform.rotation = rot;

        base.Update();

        float movementVal = sm.PlayerInput.actions["Move"].ReadValue<float>();

        float grappleVal = sm.PlayerInput.actions["GrappleLength"].ReadValue<float>();

        if (grappleVal != 0)
        {
            if (sm.transform.parent.GetComponentInChildren<TwoPointRope>().LineLength - grappleVal * GrappleExtensionSpeed * Time.deltaTime > 0.25)
            {
                sm.GetComponent<DistanceJoint2D>().distance -= grappleVal * GrappleExtensionSpeed * Time.deltaTime;
                sm.transform.parent.GetComponentInChildren<TwoPointRope>().LineLength -= grappleVal * GrappleExtensionSpeed * Time.deltaTime;
            }
        }

        sm.Rigidbody.AddForce(new Vector2(movementVal * (GrapplePower * Time.deltaTime), 0));

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FindObjectOfType<Grapple>().ReleaseGrapple();
            sm.transform.TweenRotation(Vector3.zero, 0.1f);
            sm.Transition(sm.JumpState);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            FindObjectOfType<Grapple>().ReleaseGrapple();

            sm.Transition(sm.FallState);
        }
    }

}
