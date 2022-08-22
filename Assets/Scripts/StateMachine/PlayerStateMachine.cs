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

    protected override void Update()
    {
        base.Update();

        //Debug.Log(CurrentState.GetType().ToString());
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

        Animator.SetBool("isGrounded", IsGrounded);
        Animator.SetFloat("moveValue", MoveValue *
            (InputDeviceManager.CurrentDeviceType == InputDevices.MnK && InputProvider.GetState().IsWalking ? 0.49f : 1));


        Animator.SetFloat("yVelocity", Rigidbody.velocity.y);
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
        Animator = GetComponent<Animator>();
    }

    protected override State GetInitialState() => IdleState;

    public void ParticleKick()
    {
        Vector2 pos = transform.TransformPoint(new Vector3(GetComponent<SpriteRenderer>().flipX ? -0.28f : 0.28f, -0.59f, 0));

        GameObject kickRock = Instantiate(KickRock);

        kickRock.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<SpriteRenderer>().flipX ? -300 : 300, 300));

        kickRock.transform.position = pos;

        IEnumerator wait(GameObject kr)
        {
            yield return new WaitForSeconds(3);

            Destroy(kr);
        }

        StartCoroutine(wait(kickRock));


    }
}
