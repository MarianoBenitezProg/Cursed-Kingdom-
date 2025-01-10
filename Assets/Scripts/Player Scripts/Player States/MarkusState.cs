using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkusState : State
{
    P_Behaviour _playerScript;
    P_View _viewScript;
    AbilityTimers _timersScript;

    private bool _canUseBasicAttack = true;
    private bool _canUseDamageAbility = true;
    private bool _canUseCCAbility = true;
    private bool _canUseUtilityAbility = true;

    public MarkusState(P_Behaviour playerRef, P_View viewRef, AbilityTimers timerRef)
    {
        _playerScript = playerRef;
        _viewScript = viewRef;
        _timersScript = timerRef;
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
        if (_canUseBasicAttack == true)
        {
            ProyectilePool.Instance.GetObstacle(ProjectileType.Markus1);
        }

        if (_canUseBasicAttack && !_timersScript.IsOnCooldown("MarkusBasicAttack"))
        {
            Debug.Log("Ataque Basico Markus");
            _canUseBasicAttack = false;
            _timersScript.StartCooldown("MarkusBasicAttack", () => _canUseBasicAttack = true);
        }
    }
    public void DamageAbility()
    {
        if (_canUseDamageAbility == true)
        {
            ProyectilePool.Instance.GetObstacle(ProjectileType.Markus2);
        }

        if (_canUseDamageAbility && !_timersScript.IsOnCooldown("MarkusDamage"))
        {
            Debug.Log("Habilidad de Daño Markus");
            _canUseDamageAbility = false;
            _timersScript.StartCooldown("MarkusDamage", () => _canUseDamageAbility = true);
        }
    }
    public void CCAbility()//Crowd Control. Stuns y slow
    {
        if (_canUseCCAbility == true)
        {
            ProyectilePool.Instance.GetObstacle(ProjectileType.Markus3);
        }

        if (_canUseCCAbility && !_timersScript.IsOnCooldown("MarkusCC"))
        {
            Debug.Log("Habilidad de CC Markus");
            _canUseCCAbility = false;
            _timersScript.StartCooldown("MarkusCC", () => _canUseCCAbility = true);
        }
    }

    public void UtilityAbility()
    {
        if (_canUseUtilityAbility == true)
        {
            ProyectilePool.Instance.GetObstacle(ProjectileType.Markus4);
        }

        if (_canUseUtilityAbility && !_timersScript.IsOnCooldown("MarkusUtility"))
        {
            Debug.Log("Habilidad de Utilidad Markus");
            _canUseUtilityAbility = false;
            _timersScript.StartCooldown("MarkusUtility", () => _canUseUtilityAbility = true);
        }
    }
}
