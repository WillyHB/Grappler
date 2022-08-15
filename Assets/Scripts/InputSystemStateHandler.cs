using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Input/InputHandlers/InputSystem")]
public class InputSystemStateHandler : InputStateHandler
{
    public override InputState HandleInputState(InputState state)
    {
        return state;
    }
}

