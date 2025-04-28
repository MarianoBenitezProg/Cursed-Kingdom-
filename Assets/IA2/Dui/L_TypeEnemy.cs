using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class L_TypeEnemy : MonoBehaviour
{
    // Función 1: Generador + Where + Take (Grupo 1 y Generator)
    public List<A_EnemyCount> GenerateAndFilterEnemies(int count)
    {
        var random = new System.Random();
        return Enumerable.Range(1, count).Select(i => new A_EnemyCount{Type = "Crawler",Health = random.Next(50, 150),Damage = random.Next(10, 30)})
            .Where(e => e.Damage > 15) 
            .Take(count / 2)
            .ToList();
    }


    public List<string> ProcessEnemyGroups(List<List<A_EnemyCount>> enemyGroups)
    {
        return enemyGroups.SelectMany(g => g).OrderBy(e => e.Health) .Select(e => $"{e.Type} - HP: {e.Health}").ToList();
    }


    public void EnemiesChunks(int chunkSize)
    {
        var enemies = GenerateAndFilterEnemies(100);

        for (int i = 0; i < enemies.Count; i += chunkSize)
        {
            var chunk = enemies.Skip(i) .Take(chunkSize); 

            if (chunk.Any(e => e.IsBoss))
            {
                var dict = chunk.ToDictionary(e => e.GetHashCode(), e => e); 
                Debug.Log($"Chunk {i / chunkSize} contiene jefes!");
            }
        }
    }
}
