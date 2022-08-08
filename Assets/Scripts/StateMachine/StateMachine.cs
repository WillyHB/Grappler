using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateMachine : MonoBehaviour
{
    private State CurrentState;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Transition(GetInitialState());
    }

    protected abstract State GetInitialState();

    public State Transition(State state)
    {
        if (CurrentState != null) CurrentState.OnExit();
        CurrentState = state;
        CurrentState.OnEnter(this);

        return CurrentState;

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CurrentState.Update();
    }

    protected virtual void FixedUpdate()
    {
        CurrentState.FixedUpdate();
    }
}
