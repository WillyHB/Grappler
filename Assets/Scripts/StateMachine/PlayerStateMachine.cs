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
    /*
     * Next-Gen Pixel Art - Astortion Devlog #20 - YouTube
    [Space(25)]
    public LayerMask GroundLayerMask;

    public float GroundedCheckRadius = 0.25f;

    public Rigidbody2D Rigidbody { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public Grapple Grapple { get; private set; }

    public bool IsGrounded { get; set; }


    protected override void Update()
    {
        base.Update();

        Debug.Log(CurrentState.GetType().ToString());

        IsGrounded = Physics2D.OverlapCircle(
        new Vector2(transform.position.x, transform.position.y - GetComponent<CapsuleCollider2D>().size.y / 2),
        GroundedCheckRadius, GroundLayerMask);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected void Awake()
    {
        Grapple = FindObjectOfType<Grapple>();  
        PlayerInput = GetComponent<PlayerInput>();
        Rigidbody = GetComponent<Rigidbody2D>();

    }

    protected override State GetInitialState() => IdleState;


}
