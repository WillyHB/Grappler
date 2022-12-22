using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateMachine : MonoBehaviour
{
    public State CurrentState { get; private set; }

    public Type PreviousState { get; private set; }

    protected virtual void Start()
    {
        Transition(GetInitialState());
    }

    protected abstract State GetInitialState();

    public State Transition(State state, bool allowReTranstion = false)
    {
        if (CurrentState != state || allowReTranstion)
        {
            if (CurrentState != null)
            {
                CurrentState.OnExit();
                PreviousState = CurrentState.GetType();
            }

            CurrentState = state;

            state.OnEnter(this);
        }

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
