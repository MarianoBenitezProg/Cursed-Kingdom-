using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseDos2 : SecondBossState
{
    Vector3 puntoRush;
    private float chargeCooldown = 5f; // Tiempo entre corneadas
    private float chargeTimer = 0f;
    private bool isCharging = false;
    private Vector3 chargeStartPosition;
    Animator rejasArriba;



    public void EnterState(SecondBoss boss)
    {
        boss.animatorToro.SetBool("IsCharging", false);
        boss.animatorToro.SetBool("IsAttacking", false);
        boss.animatorToro.SetBool("IsWalking", false);

        boss.transform.position = boss.spawnPoints[1].transform.position;
        boss.rb.velocity = Vector2.zero;
        boss.damage += 2;
        rejasArriba = boss.rejas[1].GetComponent<Animator>();
        rejasArriba.SetBool("openJailBool", true);
        spawnearEnemigos(boss);
    }

    #region enemysSpawn
    void spawnearEnemigos(SecondBoss boss)
    {
        GameObject enemigo1 = GameObject.Instantiate(boss.enemigsToSpawn[0], boss.spawnPoints[1].transform.position, Quaternion.identity);
        GameObject enemigo2 = GameObject.Instantiate(boss.enemigsToSpawn[0], boss.spawnPoints[1].transform.position, Quaternion.identity);
        GameObject enemigo3 = GameObject.Instantiate(boss.enemigsToSpawn[2], boss.spawnPoints[1].transform.position, Quaternion.identity);

        AsignarPath(enemigo1, new List<int> { 1, 2, 0, 4}, boss);
        AsignarPath(enemigo2, new List<int> { 1, 4, 0, 2 }, boss);
        AsignarPath(enemigo3, new List<int> {1, 3, 7, 8, 5,}, boss);

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
        boss.directionTime += Time.deltaTime;
        if(boss.directionTime >0 && boss.directionTime < 2)
        {
            boss.animatorToro.SetBool("IsCharging", false);
            boss.animatorToro.SetBool("IsAttacking", false);
            boss.animatorToro.SetBool("IsWalking", false);

        }else if (boss.directionTime < 3)
        {
            StartCharge(boss);
        }
        else if (boss.directionTime > 3 && boss.directionTime <= 7)
        {
            ChargeMove(boss);
        }
        else if (boss.directionTime > 7 && boss.directionTime < 20)
        {
            Move(boss);
        }
        else
        {
            boss.directionTime = 0;
        }
    }
    #region movimiento 1
    private void Move(SecondBoss boss)
    {
        boss.rb.velocity = Vector3.zero;

        boss.animatorToro.SetBool("IsWalking", true);
        boss.animatorToro.SetBool("IsAttacking", false);

        if (boss.player != null && boss.playerDistance > 6)
        {
            boss.dir = (boss.player.transform.position - boss.transform.position).normalized;
            boss.rb.velocity = boss.dir * boss.speed;
        }
        else
        {
            boss.animatorToro.SetBool("IsWalking", false);
            boss.rb.velocity = Vector3.zero;
            boss.atackTimer += Time.deltaTime;

            if (boss.atackTimer >= boss.atackCountDown)
            {
                shot(boss);
                boss.atackTimer = 0;
            }
            if (boss.atackTimer >= boss.atackCountDown - 1f)
            {
                boss.animatorToro.SetBool("IsAttacking", true);
            }
        }
    }
    void shot(SecondBoss boss)
    {
        var proyetile = ProyectilePool.Instance.GetObstacle(ProjectileType.SecondBossAtack);
        var toroProyec = proyetile.GetComponent<ToroProyec>();
        toroProyec.dmg = boss.damage;
        proyetile.transform.position = boss.transform.position + boss.dir * 5;
    }

    #endregion
    private void StartCharge(SecondBoss boss)
    {
        boss.animatorToro.SetBool("IsCharging", true);

        boss.rb.velocity = Vector2.zero;
        chargeStartPosition = boss.transform.position;

        boss.dir = (boss.player.transform.position - boss.transform.position).normalized;
    }

    private void ChargeMove(SecondBoss boss)
    {
        float distanciaRecorrida = Vector3.Distance(chargeStartPosition, boss.transform.position);
        float raycastDistancia = 1f;

        RaycastHit2D hitFrontal = Physics2D.Raycast(
            boss.transform.position,
            boss.dir,
            raycastDistancia,
            boss.obstacleLayer
        );
        Debug.DrawRay(boss.transform.position, boss.dir * raycastDistancia, Color.red);

        if (hitFrontal.collider != null)
        {
            Debug.Log("Obstáculo detectado, recalculando dirección...");
            boss.animatorToro.SetBool("IsCharging", false);
            boss.animatorToro.SetBool("IsAttacking", false);
            boss.animatorToro.SetBool("IsWalking", false);
            boss.rb.velocity = Vector3.zero; // Añadir reset de velocidad
            boss.directionTime = 7.5f;
        }
        else if (distanciaRecorrida < boss.allowRunDistance)
        {
            // Usar velocidad en lugar de cambiar directamente la posición
            boss.rb.velocity = boss.dir * boss.speed;
        }
        else
        {
            // Reset de velocidad al terminar el charge
            boss.rb.velocity = Vector3.zero;
        }
    }

    public void ExitState(SecondBoss boss)
    {
    }
}
