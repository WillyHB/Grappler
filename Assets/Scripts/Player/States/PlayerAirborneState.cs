using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : State
{
    protected PlayerStateMachine sm;

    public float MovementSpeed = 75;
    public float Accelerant = 25;
    private float accelerant;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm = fsm as PlayerStateMachine;

        accelerant = sm.Rigidbody.velocity.x;
    }

    public override void OnExit()
    {
        base.OnExit();;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float moveValue = sm.InputProvider.GetState().MoveDirection;

        float speed = moveValue * (MovementSpeed * Time.deltaTime);

        if (moveValue > 0)
        {
            if (accelerant < speed)
            {
                accelerant += Accelerant * Time.deltaTime;
            }
        }

        else if (moveValue < 0)
        {
            if (accelerant > speed)
            {
                accelerant -= Accelerant * Time.deltaTime;
            }
        }

        else
        {
            if (accelerant != sm.Rigidbody.velocity.x)
            {
                accelerant = sm.Rigidbody.velocity.x;
            }
        }

        sm.Rigidbody.velocity = new Vector2(accelerant, sm.Rigidbody.velocity.y);
    }
}
