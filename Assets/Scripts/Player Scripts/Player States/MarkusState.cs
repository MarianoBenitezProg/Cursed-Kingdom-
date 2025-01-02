using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkusState : State
{
    P_Behaviour _playerScript;

    public MarkusState(P_Behaviour playerRef)
    {
        _playerScript = playerRef;
    }

    protected override void OnEnter()
    {
        Debug.Log("Enter Markus State");
    }

    protected override void OnExit()
    {

    }

    protected override void OnUpdate(float deltaTime)
    {

    }

    public void SwitchToFerana()
    {
        _playerScript._fsm.SendInput("SwitchToFerana");
    }
}
