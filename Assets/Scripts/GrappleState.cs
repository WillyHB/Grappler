using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName ="States/Player/GrappleState")]
public class GrappleState : State
{
    public float GrapplePower = 50;

    private PlayerStateMachine sm;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
        sm = fsm as PlayerStateMachine;
    }

    public override void Update()
    {
        base.Update();

        float movementVal = sm.PlayerInput.actions["Move"].ReadValue<float>();

        float grappleVal = sm.PlayerInput.actions["GrappleLength"].ReadValue<float>();

        if (grappleVal != 0)
        {
            sm.GetComponent<DistanceJoint2D>().distance -= grappleVal * Time.deltaTime;
            sm.transform.parent.GetComponentInChildren<TwoPointRope>().LineLength -= grappleVal * Time.deltaTime;
        }

        sm.Rigidbody.AddForce(new Vector2(movementVal * (GrapplePower * Time.deltaTime), 0));

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FindObjectOfType<Grapple>().ReleaseGrapple();

            sm.Transition(sm.FallState);
        }
    }

}
