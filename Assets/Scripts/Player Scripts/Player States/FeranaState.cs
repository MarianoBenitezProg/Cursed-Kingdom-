using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeranaState : State
{
    P_Behaviour _playerScript;
    P_View _viewScript;
    AbilityTimers _timersScript;

    private bool _canUseBasicAttack = true;
    private bool _canUseDamageAbility = true;
    private bool _canUseCCAbility = true;
    private bool _canUseUtilityAbility = true;

    public FeranaState(P_Behaviour playerRef, P_View viewRef, AbilityTimers timerRef)
    {
        _playerScript = playerRef;
        _viewScript = viewRef;
        _timersScript = timerRef;
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
        if (_canUseBasicAttack && !_timersScript.IsOnCooldown("FeranaBasicAttack"))
        {
            Debug.Log("Ataque Basico Ferana");
            _canUseBasicAttack = false;
            _timersScript.StartCooldown("FeranaBasicAttack", () => _canUseBasicAttack = true);
        }
    }
    public void DamageAbility()
    {
        if (_canUseDamageAbility && !_timersScript.IsOnCooldown("FeranaDamage"))
        {
            Debug.Log("Habilidad de Daño Ferana");
            _canUseDamageAbility = false;
            _timersScript.StartCooldown("FeranaDamage", () => _canUseDamageAbility = true);
        }
    }
    public void CCAbility()//Crowd Control. Stuns y slow
    {
        if (_canUseCCAbility && !_timersScript.IsOnCooldown("FeranaCC"))
        {
            Debug.Log("Habilidad de CC Ferana");
            _canUseCCAbility = false;
            _timersScript.StartCooldown("FeranaCC", () => _canUseCCAbility = true);
        }
    }

    public void UtilityAbility()
    {
        if (_canUseUtilityAbility && !_timersScript.IsOnCooldown("FeranaUtility"))
        {
            Debug.Log("Habilidad de Utilidad Ferana");
            _canUseUtilityAbility = false;
            _timersScript.StartCooldown("FeranaUtility", () => _canUseUtilityAbility = true);
        }
    }
}
