using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachine
{
    public PlayerWalkState WalkState;
    public PlayerRunState RunState;
    public PlayerIdleState IdleState;

    public PlayerJumpState JumpState;
    public PlayerFallState FallState;
    public PlayerLandState LandState;
    public GrappleState GrappleState;
    public GrappleExitState GrappleExitState;

    public PlayerDuckState Duck;

    public class Anims
    {
        public int Land { get; private set; } = Animator.StringToHash("Land");
        public int RunLand { get; private set; } = Animator.StringToHash("RunLand");

        public int Idle { get; private set; } = Animator.StringToHash("Idle");
        public int Run { get; private set; } = Animator.StringToHash("Run");
        public int Walk { get; private set; } = Animator.StringToHash("Walk");
        public int Jump { get; private set; } = Animator.StringToHash("Jump");

        public int FallDown { get; private set; } = Animator.StringToHash("FallDown");
        public int FallUp { get; private set; } = Animator.StringToHash("FallUp");

        public int Duck { get; private set; } = Animator.StringToHash("Duck");
        public int DuckToIdle { get; private set; } = Animator.StringToHash("DuckToIdle");
        public int DuckFallDown { get; private set; } = Animator.StringToHash("DuckFallDown");
        public int DuckFallUp { get; private set; } = Animator.StringToHash("DuckFallUp");

        public int Grapple { get; private set; } = Animator.StringToHash("Grapple");

        #region GrappleExits
        public int GrappleSpinExit { get; private set; } = Animator.StringToHash("GrappleSpinExit");
        public int GrappleTuckedSpinExit { get; private set; } = Animator.StringToHash("GrappleTuckedSpinExit");
        public int GrappleFrontFlipExit { get; private set; } = Animator.StringToHash("GrappleFrontflipExit");
        public int GrappleBackFlipExit { get; private set; } = Animator.StringToHash("GrappleBackflipExit");
        public int GrappleUDBackFlipExit { get; private set; } = Animator.StringToHash("GrappleUpsideDownBackflipExit");
        public int GrappleJumpExit { get; private set; } = Animator.StringToHash("GrappleJumpExit");
        #endregion

        #region AirTricks
        public int SpinTrick { get; private set; } = Animator.StringToHash("SpinTrick");
        public int TuckedSpinTrick { get; private set; } = Animator.StringToHash("TuckedSpinTrick");
        public int FrontFlipTrick { get; private set; } = Animator.StringToHash("FrontFlipTrick");
        public int SideSpinTrick { get; private set; } = Animator.StringToHash("SideSpinTrick");
        #endregion
    }

    public Anims Animations { get; } = new();
    [Space(25)]
    public LayerMask GroundLayerMask;

    public float GroundedCheckRadius = 0.25f;


    public Rigidbody2D Rigidbody { get; private set; }
    public InputProvider InputProvider;
    public Animator Animator { get; private set; }

    public Grapple Grapple { get; private set; }

    public bool IsGrounded { get; set; }

    public float MoveValue {get; set;}

    public GameObject KickRock;
    public GameObject JumpDust;
    public GameObject RunDust;

    protected override void Update()
    {
        base.Update();

        Debug.Log(CurrentState.GetType().ToString());
        MoveValue = InputProvider.GetState().MoveDirection;

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

        Rigidbody.velocity = new Vector2(Mathf.Clamp(Rigidbody.velocity.x, -20, 20), Rigidbody.velocity.y);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void Awake()
    {
        Grapple = FindObjectOfType<Grapple>();  
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    protected override State GetInitialState() => IdleState;

    public void ParticleKick() => IdleState.ParticleKick();
}
