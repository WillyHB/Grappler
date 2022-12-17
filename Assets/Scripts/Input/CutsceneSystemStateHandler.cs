using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;


[CreateAssetMenu(menuName = "Input/InputHandlers/CutsceneSystem")]
public class CutsceneSystemStateHandler : InputStateHandler
{
    public InputActionAsset Actions;

    private float moveDirection;
    private bool isWalking;
    public bool IsInCutscene { get; set; }

    /*
    public void OnEnable()
    {
        Actions.FindActionMap("Player").FindAction("Jump").performed += (cc) => PerformJump();
        Actions.FindActionMap("Player").FindAction("Shoot").performed += (cc) => PerformShoot();
        Actions.FindActionMap("Player").FindAction("Grapple").performed += (cc) => PerformGrapple();
        Actions.FindActionMap("Player").FindAction("CancelGrapple").performed += (cc) => CancelGrapple();
    }

    public void OnDisable()
    {
        Actions.FindActionMap("Player").FindAction("Jump").performed -= (cc) => PerformJump();
        Actions.FindActionMap("Player").FindAction("Shoot").performed -= (cc) => PerformShoot();
        Actions.FindActionMap("Player").FindAction("Grapple").performed -= (cc) => PerformGrapple();
        Actions.FindActionMap("Player").FindAction("CancelGrapple").performed -= (cc) => CancelGrapple();
    }*/

    public async Task Move(float direction, int time, bool walk)
    {
        isWalking = walk;

        moveDirection = direction;

        await Task.Delay(time);

        moveDirection = 0;
        isWalking = false;
    }

    public override InputState HandleInputState(InputState state)
    {
        if (IsInCutscene)
        {
            state.MoveDirection = moveDirection;
            state.SwimDirection = 0;
            state.GrappleLength = 0;

            state.CanJump = false;
            state.CanGrapple = false;
            state.CanShoot = false;

            state.IsJumping = false;
            state.IsCrouching = false;
            state.IsWalking = isWalking;
        }

        return state;
    }


}

