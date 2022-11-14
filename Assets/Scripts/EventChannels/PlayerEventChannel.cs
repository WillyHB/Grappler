using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Event Channels/Player")]
public class PlayerEventChannel : ScriptableObject
{
    public event Action Die;

    public void Kill() => Die?.Invoke();
}
