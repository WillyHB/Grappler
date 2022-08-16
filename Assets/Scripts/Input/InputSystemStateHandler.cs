using System;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "Input/InputHandlers/InputSystem")]
public class InputSystemStateHandler : InputStateHandler
{
    public InputActionAsset Actions;

    public void OnEnable()
    {
        Actions.FindActionMap("Player").FindAction("Jump").started += (cc) => PerformJump();
        Actions.FindActionMap("Player").FindAction("Shoot").started += (cc) => PerformShoot();
        Actions.FindActionMap("Player").FindAction("Grapple").started += (cc) => PerformGrapple();
    }

    public void OnDisable()
    {
        
    }

    public override InputState HandleInputState(InputState state)
    {
        state.MoveDirection = Actions.FindActionMap("Player").FindAction("Move").ReadValue<float>();
        state.CanJump = true;
        state.CanGrapple = true;
        state.IsJumping = Actions.FindActionMap("Player").FindAction("Jump").IsPressed();
        state.CanShoot = false;
        state.IsCrouching = Actions.FindActionMap("Player").FindAction("Crouch").IsPressed();
        return state;
    }
}

