using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeranaState : State
{
    P_Behaviour _playerScript;
    P_View _view;
    AbilityTimers _timersScript;

    private bool _canUseBasicAttack = true;
    private bool _canUseDamageAbility = true;
    private bool _canUseCCAbility = true;
    private bool _canUseUtilityAbility = true;

    public FeranaState(P_Behaviour playerRef, P_View viewRef, AbilityTimers timerRef)
    {
        _playerScript = playerRef;
        _view = viewRef;
        _timersScript = timerRef;
    }

    protected override void OnEnter()
    {
        Debug.Log("Enter Ferana State");
        _view.SwitchToFerana();
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
        if(_canUseBasicAttack == true)
        {
            ProyectilePool.Instance.GetObstacle(ProjectileType.Ferrana1);
            _view.TriggerFeranaBasic();
        }

        if (_canUseBasicAttack && !_timersScript.IsOnCooldown("FeranaBasicAttack"))
        {
            _canUseBasicAttack = false;
            _timersScript.StartCooldown("FeranaBasicAttack", () => _canUseBasicAttack = true);
        }
    }
    public void DamageAbility()
    {
        if (_canUseDamageAbility == true)
        {
            ProyectilePool.Instance.GetObstacle(ProjectileType.Ferrana2);
            _view.TriggerFeranaSlash();
        }

        if (_canUseDamageAbility && !_timersScript.IsOnCooldown("FeranaDamage"))
        {
            _canUseDamageAbility = false;
            _timersScript.StartCooldown("FeranaDamage", () => _canUseDamageAbility = true);
        }
    }
    public void CCAbility()//Crowd Control. Stuns y slow
    {
        if (_canUseCCAbility == true)
        {
            ProyectilePool.Instance.GetObstacle(ProjectileType.Ferrana3);
            _view.TriggerFeranaSlam();
        }

        if (_canUseCCAbility && !_timersScript.IsOnCooldown("FeranaCC"))
        {
            _canUseCCAbility = false;
            _timersScript.StartCooldown("FeranaCC", () => _canUseCCAbility = true);
        }
    }

    public void UtilityAbility()
    {
        if (_canUseUtilityAbility == true)
        {
            ProyectilePool.Instance.GetObstacle(ProjectileType.Ferrana4);
            _view.TriggerFeranaDagger();
        }

        if (_canUseUtilityAbility && !_timersScript.IsOnCooldown("FeranaUtility"))
        {
            _canUseUtilityAbility = false;
            _timersScript.StartCooldown("FeranaUtility", () => _canUseUtilityAbility = true);
        }
    }
}
