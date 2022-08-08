using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachine
{
    public GrappleState GrappleState;
    public PlayerMoveState MoveState;
    public PlayerIdleState IdleState;
    public PlayerJumpState JumpState;
    public PlayerFallState FallState;

    [Space(25)]
    public LayerMask GroundLayerMask;

    public float GroundedCheckRadius = 0.25f;

    public Rigidbody2D Rigidbody { get; private set; }
    public PlayerInput PlayerInput { get; private set; }


    protected void Awake()
    {
  
        PlayerInput = GetComponent<PlayerInput>();
        Rigidbody = GetComponent<Rigidbody2D>();

    }

    protected override State GetInitialState() => IdleState;


}
