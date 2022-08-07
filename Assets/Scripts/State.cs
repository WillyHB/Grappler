using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class State : ScriptableObject
{
    public List<Transition> Transitions = new();
    public StateMachine FSM { get; set; }

    public abstract void Update();
    public abstract void OnEnter();
    public abstract void OnExit();

}

[Serializable]
public struct Transition
{
    public State State;
}
