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
            >= 225 => "GrappleJumpExit225",
            >= 180 => "GrappleJumpExit180",
            >= 90 => "GrappleJumpExit90",
            >= 45 => "GrappleJumpExit45",
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

        if ((sm.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
            || sm.IsGrounded) && !sm.Animator.GetCurrentAnimatorStateInfo(0).IsName("Grapple"))

        {
            sm.Transition(sm.FallState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
