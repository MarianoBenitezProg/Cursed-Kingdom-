using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseDos2 : SecondBossState
{
    private float chargeCooldown = 5f; // Tiempo entre corneadas
    private float chargeTimer = 0f;
    private bool isCharging = false;
    private Vector3 chargeStartPosition;


    public void EnterState(SecondBoss boss)
    {
        boss.transform.position = boss.spawnPoints[1].transform.position;
        boss.rb.velocity = Vector2.zero;
        boss.speed += 2;
        boss.damage += 2;
        spawnearEnemigos(boss);
    }

    #region enemysSpawn
    void spawnearEnemigos(SecondBoss boss)
    {
        GameObject enemigo1 = GameObject.Instantiate(boss.enemigsToSpawn[1], boss.spawnPoints[0].transform.position, Quaternion.identity);
        GameObject enemigo2 = GameObject.Instantiate(boss.enemigsToSpawn[1], boss.spawnPoints[0].transform.position, Quaternion.identity);
        GameObject enemigo3 = GameObject.Instantiate(boss.enemigsToSpawn[1], boss.spawnPoints[1].transform.position, Quaternion.identity);
        GameObject enemigo4 = GameObject.Instantiate(boss.enemigsToSpawn[1], boss.spawnPoints[1].transform.position, Quaternion.identity);

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
        boss.directionTime += Time.deltaTime;

        if (boss.directionTime < 3)
        {
            StartCharge(boss);
        }
        else if (boss.directionTime > 3 && boss.directionTime <= 7)
        {
            ChargeMove(boss);
        }
        else if (boss.directionTime > 7 && boss.directionTime < 14)
        {
            Move(boss);
        }else
        {
            boss.directionTime = 0;
        }
    }

    private void Move(SecondBoss boss)
    {

        boss.animatorToro.SetTrigger("IsWalking");

        if (boss.player != null && boss.playerDistance > 6)
        {
            boss.dir = (boss.player.transform.position - boss.transform.position).normalized;
            boss.rb.velocity = boss.dir * boss.speed;
        }
        else
        {
            boss.rb.velocity = Vector3.zero;
            boss.atackTimer += Time.deltaTime;

            if (boss.atackTimer >= boss.atackCountDown)
            {
                shot(boss);
                boss.atackTimer = 0;
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


    private void StartCharge(SecondBoss boss)
    {
        boss.animatorToro.ResetTrigger("IsCharging");

        boss.rb.velocity = Vector2.zero;
        chargeStartPosition = boss.transform.position;

        boss.dir = (boss.player.transform.position - boss.transform.position).normalized;
    }

    private void ChargeMove(SecondBoss boss)
    {
        boss.animatorToro.SetTrigger("IsCharging");

        float distanciaRecorrida = Vector3.Distance(chargeStartPosition, boss.transform.position);

        float raycastDistancia = 1f;
        RaycastHit2D hitFrontal = Physics2D.Raycast(boss.transform.position, boss.dir, raycastDistancia, boss.obstacleLayer);

        Debug.DrawRay(boss.transform.position, boss.dir * raycastDistancia, Color.red); 


        if (hitFrontal.collider != null)
        {
            Debug.Log("Obstáculo detectado, recalculando dirección...");
            boss.animatorToro.ResetTrigger("IsCharging");
            boss.directionTime = 7.5f;

        }
        else if (distanciaRecorrida < boss.allowRunDistance)
        {
            boss.transform.position += boss.dir * boss.speed * Time.deltaTime;
        }
    }

    public void ExitState(SecondBoss boss)
    {
    }
}
