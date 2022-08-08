using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName ="States/Player/MoveState")]
public class PlayerMoveState : State
{
    public float Speed = 100;



    public float StopAccelerant = 0.1f;

    public float StartAccelerant;

    private float accelerant;


    private bool isGrounded = false;

    private PlayerStateMachine sm;


    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        sm = fsm as PlayerStateMachine;

        sm.PlayerInput.actions["Jump"].started += Jump;

    }

    public override void OnExit()
    {
        base.OnExit();

        sm.PlayerInput.actions["Jump"].started -= Jump;
    }

    private void Jump(InputAction.CallbackContext cc)
    {
        sm.Transition(sm.JumpState);
    }

    public override void Update()
    {
        base.Update();

        isGrounded = Physics2D.OverlapCircle(
        new Vector2(sm.transform.position.x, sm.transform.position.y - sm.GetComponent<CapsuleCollider2D>().size.y / 2),
        sm.GroundedCheckRadius, sm.GroundLayerMask);

        if (!isGrounded)
        {
            sm.Transition(sm.FallState);
        }

    }
    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float val = sm.PlayerInput.actions["Move"].ReadValue<float>();


        if (val == 0)
        {
            if (accelerant < 0)
            {
                accelerant += Mathf.Min(StopAccelerant * Time.deltaTime, -accelerant);
            }

            else if (accelerant > 0)
            {
                accelerant -= Mathf.Min(StopAccelerant * Time.deltaTime, accelerant);
            }

        }

        else
        {
            float speed = val * (Speed * Time.deltaTime);

            if (val > 0)
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

            else if (val < 0)
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

        sm.Rigidbody.velocity = new Vector2(accelerant == 0 ? sm.Rigidbody.velocity.x : accelerant, sm.Rigidbody.velocity.y);

    }
}
