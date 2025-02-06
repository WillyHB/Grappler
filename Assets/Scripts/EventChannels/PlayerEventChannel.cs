using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Event Channels/Player")]
public class PlayerEventChannel : ScriptableObject
{
    public event Action Die;
    /// <summary>
    /// Boolean: Whether to freeze animations as well
    /// </summary>
    public event Action<bool> Frozen;
    /// <summary>
    /// Boolean: Preserve velocity
    /// </summary>
    public event Action<bool> UnFrozen;

    public void Freeze(bool freezeAnimations) => Frozen?.Invoke(freezeAnimations);
    public void UnFreeze(bool preserveVelocity) => UnFrozen?.Invoke(preserveVelocity);
    public void Kill() => Die?.Invoke();
}
