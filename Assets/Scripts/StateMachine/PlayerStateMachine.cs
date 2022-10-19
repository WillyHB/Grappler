using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public PlayerSwimmingState SwimState;

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

        public int Swim { get; private set; } = Animator.StringToHash("Swim");
        public int SwimMove { get; private set; } = Animator.StringToHash("SwimMove");

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
    public string GroundTag = "Ground";

    public float GroundedCheckRadius = 0.25f;

    [Range(0, 20)]
    public float MovementZoomMagnitude = 10;


    public Rigidbody2D Rigidbody { get; private set; }
    public InputProvider InputProvider;
    public Animator Animator { get; private set; }

    public Grapple Grapple { get; private set; }

    public bool IsGrounded { get; set; }

    public float MoveValue {get; set;}

    public Water CurrentWater { get; set; }

    public GameObject KickRock;


    protected override void Update()
    {
        base.Update();

        //Debug.Log(CurrentState);

        MoveValue = InputProvider.GetState().MoveDirection;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            new Vector2(transform.position.x, transform.position.y - GetComponent<BoxCollider2D>().size.y / 2 + GetComponent<BoxCollider2D>().offset.y),
            GroundedCheckRadius);

        bool grounded = false;

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag(GroundTag))
            {
                grounded = true;
                break;
            }
        }

        IsGrounded = grounded;

        if (InputProvider.GetState().MoveDirection > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        else if (InputProvider.GetState().MoveDirection < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        Rigidbody.velocity = new Vector2(Mathf.Clamp(Rigidbody.velocity.x, -20, 20), Rigidbody.velocity.y);

        if (CurrentWater != null) Transition(SwimState);
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
