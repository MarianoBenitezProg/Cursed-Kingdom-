using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkusState : State
{
    P_Behaviour _playerScript;
    P_View _viewScript;

    public MarkusState(P_Behaviour playerRef, P_View viewRef)
    {
        _playerScript = playerRef;
        _viewScript = viewRef;
    }

    protected override void OnEnter()
    {
        Debug.Log("Enter Markus State");
        _viewScript.SwitchToMarkus();
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

    public void BasicAttack()
    {
        Debug.Log("Ataque Basico Markus");
    }
    public void DamageAbility()
    {
        Debug.Log("Habilidad de Daño Markus");
    }
    public void CCAbility()//Crowd Control. Stuns y slow
    {
        Debug.Log("Habilidad de CC Markus");
    }

    public void UtilityAbility()
    {
        Debug.Log("Habilidad de Utilidad Markus");
    }

}
