using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="States/Player/GrappleIdle")]
public class GrappleIdleState : GrappleGroundedState
{
    public float GroundFriction = 25;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (moveValue == 0)
        {
            if (accelerant < 0)
            {
                accelerant += Mathf.Min(GroundFriction * Time.deltaTime, -accelerant);
            }

            else if (accelerant > 0)
            {
                accelerant -= Mathf.Min(GroundFriction * Time.deltaTime, accelerant);
            }

        }

        else
        {
            sm.Transition(sm.GrappleMovingState);
        }
    }
}
