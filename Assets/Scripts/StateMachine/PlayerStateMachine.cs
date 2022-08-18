using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachine
{
    public PlayerMoveState MoveState;
    public PlayerIdleState IdleState;
    public PlayerJumpState JumpState;
    public PlayerFallState FallState;

    public GrappleState GrappleState;

    [Space(25)]
    public LayerMask GroundLayerMask;

    public float GroundedCheckRadius = 0.25f;

    public Rigidbody2D Rigidbody { get; private set; }
    public InputProvider InputProvider;
    public Grapple Grapple { get; private set; }

    public bool IsGrounded { get; set; }

    protected override void Update()
    {
        base.Update();

        Debug.Log(CurrentState.GetType().ToString());

        IsGrounded = Physics2D.OverlapCircle(
        new Vector2(transform.position.x, transform.position.y - GetComponent<CapsuleCollider2D>().size.y / 2),
        GroundedCheckRadius, GroundLayerMask);

        if (InputProvider.GetState().MoveDirection > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        else if (InputProvider.GetState().MoveDirection < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();


        //transform.position = PixelClamp(transform.position, 16);
    }

    protected void Awake()
    {
        Grapple = FindObjectOfType<Grapple>();  
        Rigidbody = GetComponent<Rigidbody2D>();

    }
    protected override State GetInitialState() => IdleState;


}
