using System;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "Input/InputHandlers/InputSystem")]
public class InputSystemStateHandler : InputStateHandler
{
    public InputActionAsset Actions;

    public void OnEnable()
    {
        Actions.FindActionMap("Player").FindAction("Jump").performed += (cc) => PerformJump();
        Actions.FindActionMap("Player").FindAction("Shoot").performed += (cc) => PerformShoot();
        Actions.FindActionMap("Player").FindAction("Grapple").performed += (cc) => PerformGrapple();
        Actions.FindActionMap("Player").FindAction("CancelGrapple").performed += (cc) => CancelGrapple();
        Actions.FindActionMap("Player").FindAction("ToggleMenu").performed += (cc) => ToggleMenu();
    }

    public void OnDisable()
    {
        Actions.FindActionMap("Player").FindAction("Jump").performed -= (cc) => PerformJump();
        Actions.FindActionMap("Player").FindAction("Shoot").performed -= (cc) => PerformShoot();
        Actions.FindActionMap("Player").FindAction("Grapple").performed -= (cc) => PerformGrapple();
        Actions.FindActionMap("Player").FindAction("CancelGrapple").performed -= (cc) => CancelGrapple();
        Actions.FindActionMap("Player").FindAction("ToggleMenu").performed -= (cc) => ToggleMenu();
    }

    public override InputState HandleInputState(InputState state)
    {
        state.MoveDirection = Actions.FindActionMap("Player").FindAction("Move").ReadValue<float>();
        state.SwimDirection = Actions.FindActionMap("Player").FindAction("VerticalMove").ReadValue<float>();
        state.GrappleLength = Actions.FindActionMap("Player").FindAction("GrappleLength").ReadValue<float>();

        state.CanJump = true;
        state.CanGrapple = true;
        state.CanShoot = true;
        state.CanToggleMenu = true;

        state.IsJumping = Actions.FindActionMap("Player").FindAction("Jump").IsPressed();
        state.IsCrouching = Actions.FindActionMap("Player").FindAction("VerticalMove").ReadValue<float>() < 0;
        state.IsWalking = Actions.FindActionMap("Player").FindAction("Walk").IsPressed() 
            || ((state.MoveDirection > 0 && state.MoveDirection < 0.5f) || (state.MoveDirection < 0 && state.MoveDirection > -0.5f));
        return state;
    }
}

