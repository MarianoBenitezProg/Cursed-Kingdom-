using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeranaState : State
{
    P_Behaviour _playerScript;
    P_View _viewScript;

    public FeranaState(P_Behaviour playerRef, P_View viewRef)
    {
        _playerScript = playerRef;
        _viewScript = viewRef;
    }

    protected override void OnEnter()
    {
        Debug.Log("Enter Ferana State");
        _viewScript.SwitchToFerana();
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
    }

    public void BasicAttack()
    {
        Debug.Log("Ataque Basico Ferana");
    }
    public void DamageHability()
    {
        Debug.Log("Habilidad de Daño Ferana");
    }
    public void CCHability()//Crowd Control. Stuns y slow
    {
        Debug.Log("Habilidad de CC Ferana");
    }

    public void UtilityHability()
    {
        Debug.Log("Habilidad de Utilidad Ferana");
    }
}
