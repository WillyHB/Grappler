using System;
using UnityEngine;

public abstract class InputStateHandler : ScriptableObject
{
    public abstract InputState HandleInputState(InputState state);

    public event Action Jumped;
    public event Action Shot;
    public event Action Grappled;

    public void PerformJump() => Jumped?.Invoke();

    public void PerformShoot() => Shot?.Invoke();
    public void PerformGrapple() => Grappled?.Invoke();
}
