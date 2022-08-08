using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="States/Player/FallState")]
public class PlayerFallState : State
{
    PlayerStateMachine sm;

    private bool isGrounded;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm = fsm as PlayerStateMachine;
    }

    public override void Update()
    {
        base.Update();

        isGrounded = Physics2D.OverlapCircle(
new Vector2(sm.transform.position.x, sm.transform.position.y - sm.GetComponent<CapsuleCollider2D>().size.y / 2),
sm.GroundedCheckRadius, sm.GroundLayerMask);

        if (isGrounded)
        {
            sm.Transition(sm.IdleState);
        }
    }
}
