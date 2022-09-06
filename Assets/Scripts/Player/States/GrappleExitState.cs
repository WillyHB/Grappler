using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="States/Player/GrappleExit")]
public class GrappleExitState : PlayerAirborneState
{
    public float JumpForce = 300;

    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        float rot = sm.transform.eulerAngles.z % 360;

        
        if (sm.GetComponent<SpriteRenderer>().flipX) rot = 360 - rot;

            Debug.Log(rot);

        sm.Animator.Play(rot switch
        {
            >= 340 => "GrappleJumpExit0",
            >= 320 => "GrappleJumpExit280",
            >= 240 => "GrappleJumpExit240",
            >= 225 => "GrappleJumpExit225",
            >= 135 => "GrappleJumpExit180",
            //>= 70 => "GrappleJumpExit90",
            >= 35 => "GrappleJumpExit45",
            >= 33 => "GrappleJumpExit280",
            >= 15 => "GrappleJumpExit25",
            >= 0 => "GrappleJumpExit0",
            _ => throw new System.Exception($"Angle {rot} does not exist wtf"),

        });
        //sm.Animator.Play($"GrappleJumpExit{rot - (rot % 45)}");
        sm.transform.rotation = Quaternion.identity;

        sm.Rigidbody.AddForce(Vector2.up * JumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (sm.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
                && !sm.Animator.GetCurrentAnimatorStateInfo(0).IsName("Grapple"))
        {
            sm.Transition(sm.FallState);
            return;
        }

        if (sm.IsGrounded)
        {
            sm.Transition(sm.LandState);
            return;
        }

        if (sm.Grapple.IsGrappling)
        {
            sm.Transition(sm.GrappleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
