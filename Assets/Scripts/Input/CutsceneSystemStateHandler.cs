using System;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "Input/InputHandlers/CutsceneSystem")]
public class CutsceneSystemStateHandler : InputStateHandler
{
    public InputActionAsset Actions;

    public bool IsInCutscene;
    public bool BlockInput;

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
    }

    public override InputState HandleInputState(InputState state)
    {
        if (BlockInput && IsInCutscene)
        {
            state.MoveDirection = 0;
            state.SwimDirection = 0;
            state.GrappleLength = 0;

            state.CanJump = false;
            state.CanGrapple = false;
            state.CanShoot = false;

            state.IsJumping = false;
            state.IsCrouching = false;
            state.IsWalking = false;
        }

        return state;
    }


}

