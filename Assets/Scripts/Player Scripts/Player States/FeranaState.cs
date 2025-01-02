using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeranaState : State
{
    P_Behaviour _playerScript;
    public FeranaState(P_Behaviour playerRef)
    {
        _playerScript = playerRef;
    }

    protected override void OnEnter()
    {
        Debug.Log("Enter Ferana State");
    }

    protected override void OnExit()
    {

    }

    protected override void OnUpdate(float deltaTime)
    {

    }
    public void SwitchToMarkus()
    {
        _playerScript._fsm.SendInput("SwitchToMarkus");
        Debug.Log("Entro Change State");
    }
}
