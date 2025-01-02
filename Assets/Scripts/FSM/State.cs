using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{

    Dictionary<string, State> transitions = new Dictionary<string, State>();

    public bool HasInput(string input)
    {
        return transitions.ContainsKey(input);
    }

    public State GetState(string input)
    {
        return transitions[input];
    }

    public void AddTransition(string _input, State _state)
    {
        if (!transitions.ContainsKey(_input))
        {
            transitions.Add(_input, _state);
        }
    }
    public void Enter()
    {
        OnEnter();
    }

    public void Exit()
    {
        OnExit();
    }

    public void Update(float deltaTime)
    {
        OnUpdate(deltaTime);
    }

    protected abstract void OnEnter();
    protected abstract void OnExit();
    protected abstract void OnUpdate(float deltaTime);
}
