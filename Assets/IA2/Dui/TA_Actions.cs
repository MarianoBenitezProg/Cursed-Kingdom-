using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TA_Actions : MonoBehaviour
{
    
    public (A_EnemyCount attacker, List<A_EnemyCount> targets, int totalDamage) ExecuteAttack(A_EnemyCount attacker, List<A_EnemyCount> targets)
    {
        int damage = attacker.Damage * targets.Count;
        return (attacker, targets, damage);
    }

    public object GetEnemyActionSummary(A_EnemyCount enemy)
    {
        return new
        {
            enemy.Type,
            ActionType = enemy.Damage > 20 ? "Ataque fuerte" : "Ataque débil",
            Timestamp = DateTime.Now
        };
    }
}
