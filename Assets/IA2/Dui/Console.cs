using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Console : MonoBehaviour
{
    private List<A_EnemyCount> enemies;
    private A_EnemyManager enemyManager;
    private L_TypeEnemy linqOps;
    private TA_Actions gameActions;

    private void Start()
    {
        InitializeEnemies();
        ShowMenu();
    }

    private void InitializeEnemies()
    {
        enemies = new List<A_EnemyCount>
        {
            new A_EnemyCount { Type = "Crawler", Health = 100, Damage = 20, IsBoss = false },
            new A_EnemyCount { Type = "Paladin", Health = 200, Damage = 35, IsBoss = true },
            new A_EnemyCount { Type = "Eye", Health = 80, Damage = 15, IsBoss = false }
        };

        enemyManager = new A_EnemyManager(enemies);
        linqOps = new L_TypeEnemy();
        gameActions = new TA_Actions();

        Debug.Log("¡Bienvenido al Sistema de Gestión de Enemigos!");
        Debug.Log("Presiona una tecla en la ventana del juego:");
        Debug.Log("[G] Generar nuevos enemigos");
        Debug.Log("[S] Mostrar estadísticas por tipo");
        Debug.Log("[A] Ejecutar ataque aleatorio");
        Debug.Log("[Q] Salir");
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateEnemies();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ShowStats();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ExecuteRandomAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }
    }

    private void GenerateEnemies()
    {
        var generatedEnemies = linqOps.GenerateAndFilterEnemies(5);
        Debug.Log("Enemigos generados:");
        foreach (var enemy in generatedEnemies)
        {
            Debug.Log($"- {enemy.Type} (Daño: {enemy.Damage}, Salud: {enemy.Health})");
        }
    }

    private void ShowStats()
    {
        var stats = enemyManager.GetEnemyStatsByType();
        Debug.Log("Estadísticas por tipo:");
        foreach (var stat in stats)
        {
            Debug.Log($"- {stat.Type}: Salud Total={stat.Health}, Daño Promedio={stat.Damage:N1}, Jefes={stat.IsBoss}");
        }
    }

    private void ExecuteRandomAttack()
    {
        var random = new System.Random();
        var attacker = enemies[random.Next(enemies.Count)];
        var target = enemies[random.Next(enemies.Count)];
        var attackResult = gameActions.ExecuteAttack(attacker, new List<A_EnemyCount> { target });

        Debug.Log($"¡{attacker.Type} atacó a {target.Type}!");
        Debug.Log($"Daño total: {attackResult.totalDamage}");
    }

    private void QuitGame()
    {
        Debug.Log("¡Gracias por usar el sistema!");
        Application.Quit(); 
    }

    private void ShowMenu()
    {
        
        Debug.Log("Opciones: [G] Generar | [S] Estadísticas | [A] Atacar | [Q] Salir");
    }
}
