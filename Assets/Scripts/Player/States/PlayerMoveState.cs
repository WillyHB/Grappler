using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerMoveState : GroundedState
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

        float speed = sm.MoveValue * (Speed * Time.deltaTime);

        if (sm.MoveValue > 0)
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

        else if (sm.MoveValue < 0)
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
    }
}
