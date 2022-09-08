using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="States/Player/AirTricks")]
public class PlayerAirTrickState : PlayerAirborneState
{
    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void Update()
    {
        base.Update();

        if (sm.IsGrounded)
        {
            sm.Transition(sm.LandState);
            return;
        }

        if (sm.Grapple.IsGrappling)
        {
            sm.Transition(sm.GrappleState);
            return;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
