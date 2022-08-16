using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/IdleState")]
public class PlayerIdleState : GroundedState
{
    public float GroundFriction = 25;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);
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
            sm.Transition(sm.MoveState);
        }
    }
}
