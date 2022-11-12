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

        RaycastHit2D[] hit;
        hit = Physics2D.RaycastAll(sm.transform.position, sm.GetComponent<SpriteRenderer>().flipX ? Vector2.left : Vector2.right, sm.GetComponent<BoxCollider2D>().bounds.size.x / 2 + sm.GetComponent<BoxCollider2D>().offset.x + 0.1f);

        bool hasHit = false;

        foreach (var h in hit)
        {
            if (h.transform.CompareTag("Ground"))
            {
                hasHit = true;
            }

        }

        if (hasHit && sm.CurrentState != sm.LandState)
        {
            sm.Animator.Play(sm.Animations.Push);
        }

        else if (sm.Grapple.IsGrappling && sm.Grapple.ConnectionRope.IsStretched() && sm.CurrentState != sm.LandState)
        {
            sm.Animator.Play(sm.Animations.Pull);
        }

        else
        {
            sm.Transition(sm.CurrentState);
        }
        
        

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
