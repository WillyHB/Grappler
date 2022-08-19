using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName ="States/Player/MoveState")]
public class PlayerMoveState : GroundedState
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
