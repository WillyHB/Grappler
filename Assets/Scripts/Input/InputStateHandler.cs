using System;
using UnityEngine;

public abstract class InputStateHandler : ScriptableObject
{
    public abstract InputState HandleInputState(InputState state);

    public event Action Jumped;
    public event Action Shot;
    public event Action Grappled;
    public event Action GrappleCanceled;
    public event Action MenuToggled;

    public void ToggleMenu() => MenuToggled?.Invoke();
    public void PerformJump() => Jumped?.Invoke();

    public void PerformShoot() => Shot?.Invoke();
    public void PerformGrapple() => Grappled?.Invoke();
    public void CancelGrapple() => GrappleCanceled?.Invoke();
}
