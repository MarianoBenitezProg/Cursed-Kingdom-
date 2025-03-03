using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseDos2 : SecondBossState
{
    private Vector3 chargeStartPosition;
    private bool isInChargeSequence = false;
    private float chargeSequenceTimer = 0f;
    private float nextChargeTime = 5f; // Tiempo entre secuencias de carga

    public void EnterState(SecondBoss boss)
    {
        boss.transform.position = boss.spawnPoints[1].transform.position;
        boss.damage += 3; // Incrementamos el daño un poco más
        boss.directionTime = 0f;
        isInChargeSequence = false;
        spawnearEnemigos(boss);
        Debug.Log("Entrando en Fase Dos");
    }

    #region enemysSpawn
    void spawnearEnemigos(SecondBoss boss)
    {
        // Spawn de una mezcla de enemigos de fase 1 y nuevos
        GameObject enemigo1 = GameObject.Instantiate(boss.enemigsToSpawn[1], boss.spawnPoints[0].transform.position, Quaternion.identity);
        GameObject enemigo2 = GameObject.Instantiate(boss.enemigsToSpawn[1], boss.spawnPoints[0].transform.position, Quaternion.identity);
        GameObject enemigo3 = GameObject.Instantiate(boss.enemigsToSpawn[0], boss.spawnPoints[1].transform.position, Quaternion.identity);
        GameObject enemigo4 = GameObject.Instantiate(boss.enemigsToSpawn[0], boss.spawnPoints[1].transform.position, Quaternion.identity);

        AsignarPath(enemigo1, new List<int> { 7, 3, 2, 1, 8 }, boss);
        AsignarPath(enemigo2, new List<int> { 8, 1, 2, 3, 7 }, boss);
        AsignarPath(enemigo3, new List<int> { 5, 1, 2, 3, 6 }, boss);
        AsignarPath(enemigo4, new List<int> { 6, 3, 2, 1, 5 }, boss);
    }

    void AsignarPath(GameObject enemigo, List<int> indicesPath, SecondBoss boss)
    {
        Enemy enemyScript = enemigo.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.path.Clear();
            foreach (int index in indicesPath)
            {
                if (index >= 0 && index < boss.paths.Count)
                {
                    enemyScript.path.Add(boss.paths[index].transform);
                }
            }
        }
    }
    #endregion

    public void UpdateState(SecondBoss boss)
    {
        if (!isInChargeSequence)
        {
            // Comportamiento de la fase inicial (moverse y disparar)
            MoveThenShoot(boss);

            // Contador para iniciar secuencia de carga
            chargeSequenceTimer += Time.deltaTime;
            if (chargeSequenceTimer >= nextChargeTime)
            {
                isInChargeSequence = true;
                boss.directionTime = 0f;
                chargeSequenceTimer = 0f;
                Debug.Log("Iniciando secuencia de carga");
            }
        }
        else
        {
            // Secuencia de carga (Fase Uno)
            boss.directionTime += Time.deltaTime;

            if (boss.directionTime < 2) // Tiempo reducido para calcular dirección
            {
                CalculateChargeDirection(boss);
            }
            else if (boss.directionTime >= 2 && boss.directionTime < 5) // Tiempo de carga
            {
                ChargeMove(boss);
            }
            else
            {
                // Fin de la secuencia de carga
                isInChargeSequence = false;
                boss.directionTime = 0f;
                boss.rb.velocity = Vector2.zero; // Detener al boss después de la carga
                Debug.Log("Secuencia de carga finalizada");
            }
        }
    }

    private void MoveThenShoot(SecondBoss boss)
    {
        // Comportamiento similar a FaseInicial2
        if (boss.player != null && boss.playerDistance > 5) // Reducción de distancia
        {
            boss.dir = (boss.player.transform.position - boss.transform.position).normalized;
            boss.rb.velocity = boss.dir * boss.speed;
        }
        else
        {
            boss.rb.velocity = Vector3.zero;
            boss.atackTimer += Time.deltaTime;

            if (boss.atackTimer >= boss.atackCountDown * 0.7f) // Reducción de tiempo entre disparos
            {
                Shoot(boss);
                boss.atackTimer = 0;
            }
        }
    }

    void Shoot(SecondBoss boss)
    {
        // Ahora dispara 2 proyectiles en ángulos ligeramente diferentes
        Vector3 dirPrincipal = boss.dir;
        Vector3 dirSecundaria = Quaternion.Euler(0, 0, 20) * dirPrincipal; // 20 grados a la derecha

        // Primer proyectil
        var proyectile1 = ProyectilePool.Instance.GetObstacle(ProjectileType.SecondBossAtack);
        var toroProyec1 = proyectile1.GetComponent<ToroProyec>();
        toroProyec1.dmg = boss.damage;
        proyectile1.transform.position = boss.transform.position + dirPrincipal * 3;

        // Segundo proyectil
        var proyectile2 = ProyectilePool.Instance.GetObstacle(ProjectileType.SecondBossAtack);
        var toroProyec2 = proyectile2.GetComponent<ToroProyec>();
        toroProyec2.dmg = boss.damage;
        proyectile2.transform.position = boss.transform.position + dirSecundaria * 3;
    }

    private void CalculateChargeDirection(SecondBoss boss)
    {
        if (boss.player != null)
        {
            chargeStartPosition = boss.transform.position;
            boss.dir = (boss.player.transform.position - boss.transform.position).normalized;
            boss.transform.rotation = Quaternion.LookRotation(Vector3.forward, boss.dir);

            // Visual de preparación para la carga
            // Aquí podrías añadir efectos visuales, partículas, etc.
        }
    }

    private void ChargeMove(SecondBoss boss)
    {
        float distanciaRecorrida = Vector3.Distance(chargeStartPosition, boss.transform.position);
        float raycastDistancia = 1.5f; // Aumentado para detección de obstáculos
        RaycastHit2D hitFrontal = Physics2D.Raycast(boss.transform.position, boss.dir, raycastDistancia, boss.obstacleLayer);

        Debug.DrawRay(boss.transform.position, boss.dir * raycastDistancia, Color.red);

        if (hitFrontal.collider != null)
        {
            // Si choca contra un obstáculo, terminamos la secuencia de carga
            boss.directionTime = 5f; // Establece el tiempo para terminar la carga
            Debug.Log("Carga interrumpida por obstáculo");
        }
        else if (distanciaRecorrida < boss.allowRunDistance * 1.5f) // Distancia de carga aumentada
        {
            // Velocidad de carga aumentada
            boss.transform.position += boss.dir * (boss.speed * 1.5f) * Time.deltaTime;

            // Comprobar colisión con el jugador durante la carga
            Collider2D hitPlayer = Physics2D.OverlapCircle(boss.transform.position, 1.2f, LayerMask.GetMask("Player"));
            if (hitPlayer != null)
            {
                Debug.Log("golpíe un enemigo");
                boss.directionTime = 5f;
            }
        }
        else
        {
            // Si completa la distancia máxima de carga, terminamos la secuencia
            boss.directionTime = 5f;
        }
    }

    public void ExitState(SecondBoss boss)
    {
        boss.rb.velocity = Vector2.zero;
        Debug.Log("Saliendo de la Fase Dos...");
    }
}