using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class StateMachine : MonoBehaviour
{
    public List<State> States = new List<State>();

    public State DefaultState;

    private State CurrentState;

    // Start is called before the first frame update
    void Awake()
    {
        CurrentState = CurrentState != null ? CurrentState : DefaultState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
