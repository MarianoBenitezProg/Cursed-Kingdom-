using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("estoy en modo ataque");
    }
    public void UpdateState(Enemy enemy)
    {
        enemy.Atack();
    }
    public void ExitState(Enemy enemy)
    {

    }
}
