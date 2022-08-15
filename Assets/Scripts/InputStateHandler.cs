using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputStateHandler : ScriptableObject
{
    public abstract InputState HandleInputState(InputState state);
}
