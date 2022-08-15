using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input/InputProvider")]
public class InputProvider : ScriptableObject
{
    public List<InputStateHandler> handlers = new List<InputStateHandler>();

    public event Action Jumped;
    public event Action Shot;
    public event Action Grappled;

    public InputState GetState()
    {
        InputState state = new();

        return state;
    }
}


public struct InputState
{
    public float MoveDirection { get; set; }
    public bool IsCrouching { get; set; }
    public bool CanJump { get; set; }

}