using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Event Channels/Void")]
public class VoidEventChannel : ScriptableObject
{
    public event Action Raised;

    public void RaiseEvent()
    {
        Raised?.Invoke();
    }
}
