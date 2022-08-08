using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="States/Player/FallState")]
public class PlayerFallState : State
{
    PlayerStateMachine sm;

    public float MoveForce = 10; 

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm = fsm as PlayerStateMachine;
    }

    public override void Update()
    {
        base.Update();

        if (sm.IsGrounded)
        {
            sm.Transition(sm.IdleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float val = sm.PlayerInput.actions["Move"].ReadValue<float>();

        //sm.Rigidbody.AddForce(new Vector2(val * MoveForce, 0));
    }
}
