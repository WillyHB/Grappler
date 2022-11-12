using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/IdleState")]
public class PlayerIdleState : GroundedState
{
    [Header("Idle Animations")]
    public float TimeBetweenAnimations = 10;
    [Space(10)]
    public GameObject KickRock;
    public Vector2 RockKickPower = new Vector2(200, 200);

    private float animationTimer;

    private int prevAnim;


    public override void OnEnter(StateMachine fsm)
    {
        base.OnEnter(fsm);

        animationTimer = 0;
        if (sm.PreviousState == typeof(PlayerDuckState))
        {
            sm.Animator.Play(sm.Animations.DuckToIdle);
        }

        else
        {
            sm.Animator.Play(sm.Animations.Idle);
        }
    }

    public override void Update()
    {
        base.Update();

        if (sm.InputProvider.GetState().IsCrouching)
        {
            sm.Transition(sm.Duck);
            return;
        }

        if (sm.MoveValue != 0)
        {
            if (sm.InputProvider.GetState().IsWalking)
            {
                sm.Transition(sm.WalkState);
                return;
            }
            
            sm.Transition(sm.RunState);
            return;
        }

        if (sm.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == sm.Animations.Idle)
        {
            animationTimer += Time.deltaTime;

            if (animationTimer >= TimeBetweenAnimations)
            {
                int anim = prevAnim;

                while (anim == prevAnim)
                {
                    anim = Random.Range(1, 7);
                }


                sm.Animator.Play($"IdleAnimation{anim}");

                animationTimer = 0;

                prevAnim = anim;
            }
        }   
    }

    public void ParticleKick()
    {
        Vector2 pos = sm.transform.TransformPoint(new Vector3(sm.GetComponent<SpriteRenderer>().flipX ? -0.28f : 0.28f, -0.59f, 0));

        GameObject kickRock = Instantiate(KickRock);

        kickRock.GetComponent<Rigidbody2D>().AddForce(
            new Vector2(sm.GetComponent<SpriteRenderer>().flipX ? -RockKickPower.x : RockKickPower.x, RockKickPower.y));

        kickRock.transform.position = pos;

        static async void wait(GameObject kr)
        {
            await System.Threading.Tasks.Task.Delay(3000);

            Destroy(kr);
        }

        wait(kickRock);  
    }
}
