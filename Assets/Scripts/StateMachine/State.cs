 using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class State : ScriptableObject
{
    //public List<Transition> Transitions = new();
    //public virtual StateMachine FSM { get; set; }

    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void OnEnter(StateMachine fsm) { }
    public virtual void OnExit() { }

}

[Serializable]
public struct Transition
{
    public State State;
}
