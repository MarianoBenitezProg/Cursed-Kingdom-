using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Console : MonoBehaviour
{
    public static void Main()
    {
       
        var enemies = new List<A_EnemyCount>
        {
            new A_EnemyCount { Type = "Crawler", Health = 100, Damage = 20, IsBoss = false },
            new A_EnemyCount { Type = "Eye", Health = 200, Damage = 35, IsBoss = false }
        };

      
        var enemyManager = new A_EnemyManager(enemies);
        var stats = enemyManager.GetEnemyStatsByType();
        foreach (var stat in stats)
        {
            Debug.Log ($"Tipo: {stat.Type}, Jefes: {stat.IsBoss}"); 
        }

        
        var linqOps = new L_TypeEnemy();
        var filteredEnemies = linqOps.GenerateAndFilterEnemies(10);
        linqOps.ProcessEnemiesInChunks(3);

        
        var gameActions = new TA_Actions();
        var attackInfo = gameActions.ExecuteAttack(enemies[0], new List<A_EnemyCount> { enemies[1] });
        Debug.Log($"Daño total del ataque: {attackInfo.totalDamage}");

        var actionSummary = gameActions.GetEnemyActionSummary(enemies[1]);
        Debug.Log($"Resumen de acción: {actionSummary}");
    }
}
