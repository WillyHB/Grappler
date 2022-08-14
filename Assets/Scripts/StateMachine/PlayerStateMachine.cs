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


        //transform.position = PixelClamp(transform.position, 16);
    }

    protected void Awake()
    {
        Grapple = FindObjectOfType<Grapple>();  
        PlayerInput = GetComponent<PlayerInput>();
        Rigidbody = GetComponent<Rigidbody2D>();

    }

    private Vector2 PixelClamp(Vector2 vector, float ppu)
    {
        Vector2 vectorInPixels = new Vector2(
            Mathf.RoundToInt(vector.x * ppu),
            Mathf.RoundToInt(vector.y * ppu));

        return vectorInPixels / ppu;
    }

    protected override State GetInitialState() => IdleState;


}
