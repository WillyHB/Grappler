using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerStateMachine : StateMachine
{
    [Space(25)]
    public PlayerWalkState WalkState;
    public PlayerRunState RunState;
    public PlayerIdleState IdleState;

    public PlayerJumpState JumpState;
    public PlayerFallState FallState;
    public PlayerLandState LandState;
    public GrappleState GrappleState;
    public GrappleExitState GrappleExitState;
    public PlayerSwimmingState SwimState;
    public PlayerEmoteState EmoteState;

    public PlayerDeathState DeathState;

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

        public int Pull { get; private set; } = Animator.StringToHash("Pull");
        public int Push { get; private set; } = Animator.StringToHash("Push");

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

    public GroundSoundManager GroundSoundManager;

    public bool IsFrozen { get; private set; }

    public bool HasDied { get; private set; }

    public PlayerEventChannel PlayerEventChannel;
    public AudioEventChannel PlayerAudioEventChannel;

    private Vector2 frozenVelocity;
    public void Freeze()
    {
        frozenVelocity = Rigidbody.velocity;
        Rigidbody.velocity = Vector2.zero;
        Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        IsFrozen = true;
        Animator.speed = 0;
    }

    public void UnFreeze(bool preserveVelocity = false)
    {
        Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        Rigidbody.velocity = preserveVelocity ? frozenVelocity : Vector2.zero;
        IsFrozen = false;
        Animator.speed = 1;
    }

    protected override void Update()
    {
        base.Update();

        if (HasDied) return;

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

        if (IsFrozen) Rigidbody.velocity = Vector2.zero;
        else Rigidbody.velocity = new Vector2(Mathf.Clamp(Rigidbody.velocity.x, -20, 20), Rigidbody.velocity.y);

        if (CurrentWater != null) Transition(SwimState);

        Debug.Log(CurrentState.GetType().ToString());
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void Awake()
    {
        PlayerEventChannel.Die += () =>
        {
            Transition(DeathState);
            HasDied = true;
        };

        Grapple = FindObjectOfType<Grapple>();  
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        //transform.position = FindObjectOfType<RoomManager>().rooms[GameData.Load().Checkpoint].Checkpoint.position;
        transform.position = FindObjectOfType<RoomManager>().rooms[14].Checkpoint.position;

        Transition(IdleState);
    }

    public void PlayFootstep()
    {
        Audio[] footsteps = GroundSoundManager.GetCurrentTileSounds()?.Footsteps;

        if (footsteps == null)
            return;

        PlayerAudioEventChannel.Play(footsteps[Random.Range(0, footsteps.Length-1)]);
    }

    protected void OnDisable()
    {
        PlayerEventChannel.Die -= () =>
        {
            Transition(DeathState);
            HasDied = true;
        };
    }

    protected override State GetInitialState() => IdleState;

    public void ParticleKick() => IdleState.ParticleKick();
}
