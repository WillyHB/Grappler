using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input/InputProvider")]
public class InputProvider : ScriptableObject
{
    public List<InputStateHandler> handlers = new List<InputStateHandler>();

    private void OnEnable()
    {
        foreach (var handler in handlers)
        {
            handler.Jumped += PerformJump;
            handler.Shot += PerformShoot;
            handler.Grappled += PerformGrapple;
            handler.GrappleCanceled += CancelGrapple;
            handler.MenuToggled += ToggleMenu;
        }
    }

    private void OnDisable()
    {
        foreach (var handler in handlers)
        {
            handler.Jumped -= PerformJump;
            handler.Shot -= PerformShoot;
            handler.Grappled -= PerformGrapple;
            handler.MenuToggled -= ToggleMenu;
        }
    }

    public event Action Jumped;
    public event Action Shot;
    public event Action Grappled;
    public event Action GrappleCanceled;
    public event Action MenuToggled;

    public void PerformJump()
    {
        if (GetState().CanJump) Jumped?.Invoke();
    }

    public void PerformShoot()
    {
        if (GetState().CanShoot) Shot?.Invoke();
    }

    public void PerformGrapple()
    {
        if (GetState().CanGrapple) Grappled?.Invoke();
    }

    public void CancelGrapple()
    {
        GrappleCanceled?.Invoke();
    }

    public void ToggleMenu() 
    {
        if (GetState().CanToggleMenu) MenuToggled?.Invoke();
    }

    public InputState GetState()
    {
        InputState state = new();

        handlers.ForEach(handler => state = handler.HandleInputState(state));

        return state;
    }
}


public struct InputState
{
    public float MoveDirection { get; set; }
    public float SwimDirection { get; set; }
    public float GrappleLength { get; set; }

    public bool IsJumping { get; set; }
    public bool IsCrouching { get; set; }
    public bool IsWalking { get; set; }

    public bool CanJump { get; set; }
    public bool CanShoot { get; set; }
    public bool CanGrapple { get; set; }
    public bool CanToggleMenu {get; set;}

}