using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class A_EnemyManager : MonoBehaviour
{
    private List<A_EnemyCount> _enemies;

    public A_EnemyManager(List<A_EnemyCount> enemies)
    {
        _enemies = enemies;
    }


    public List<A_EnemyCount> GetEnemyStatsByType()
    {
        return _enemies
        .GroupBy(e => e.Type)
        .Select(g => new A_EnemyCount
        {
            Type = g.Key,
            Health = g.Sum(e => e.Health),
            Damage = (int)g.Average(e => e.Damage),
            IsBoss = g.Any(e => e.IsBoss)
        })
        .ToList();
    }
}
