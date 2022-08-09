using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/GrappleMoving")]
public class GrappleMovingState : GrappleGroundedState
{
    public float Speed = 100;

    public float StartAccelerant;

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

        if (!sm.IsGrounded)
        {
            sm.Transition(sm.GrappleState);
        }
    }
    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float speed = moveValue * (Speed * Time.deltaTime);

        if (moveValue > 0)
        {
            if (accelerant < speed)
            {
                accelerant += StartAccelerant * Time.deltaTime;
            }

            else
            {
                accelerant = speed;
            }


        }

        else if (moveValue < 0)
        {
            if (accelerant > speed)
            {
                accelerant -= StartAccelerant * Time.deltaTime;
            }

            else
            {
                accelerant = speed;
            }

        }

        else
        {
            sm.Transition(sm.IdleState);

        }

    }
}
