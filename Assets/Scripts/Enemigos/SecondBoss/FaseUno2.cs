using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseUno2 : SecondBossState
{
    Vector3 puntoRush;
    Animator rejasArriba;
    public void EnterState(SecondBoss boss)
    {
        boss.rb.velocity = Vector2.zero;
        boss.transform.position =  boss.originPoint;
        boss.damage += 2;
        boss.speed += 2;
        rejasArriba = boss.rejas[0].GetComponent<Animator>();
        rejasArriba.SetBool("openJailBool", true);
        spawnearEnemigos(boss);

    }


    #region spawnEnemys
    void spawnearEnemigos(SecondBoss boss)
    {
        GameObject enemigo1 = GameObject.Instantiate(boss.enemigsToSpawn[0], boss.spawnPoints[0].transform.position, Quaternion.identity);
        GameObject enemigo2 = GameObject.Instantiate(boss.enemigsToSpawn[0], boss.spawnPoints[0].transform.position, Quaternion.identity);
        GameObject enemigo3 = GameObject.Instantiate(boss.enemigsToSpawn[1], boss.spawnPoints[0].transform.position, Quaternion.identity);

        AsignarPath(enemigo1, new List<int> { 0, 2, 1, 4}, boss);
        AsignarPath(enemigo2, new List<int> { 0, 4, 1, 2}, boss);
        AsignarPath(enemigo3, new List<int> { 0, 3, 5, 8, 7, 6 }, boss);
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
        calculateDirect(boss);
        }else if(boss.directionTime > 3 && boss.directionTime <= 7)
        {
        Move(boss);
        }else if(boss.directionTime > 7)
        {
            boss.directionTime = 0;
        }

    }


    public void calculateDirect(SecondBoss boss)
    {
        boss.animatorToro.SetBool("IsWalking", true);
        boss.animatorToro.SetBool("IsCharging", true);

        if (boss.player != null)
        {
            boss.dir = (boss.player.transform.position - boss.transform.position).normalized;
            puntoRush = boss.transform.position;

        }
    }

    private void Move(SecondBoss boss)
    {
        boss.animatorToro.SetBool("IsWalking", false);
        float distanciaRecorrida = Vector3.Distance(puntoRush, boss.transform.position);

        #region raycast para obstaculos
        float raycastDistancia = 1f; 
        RaycastHit2D hitFrontal = Physics2D.Raycast(boss.transform.position, boss.dir, raycastDistancia, boss.obstacleLayer);
        Debug.DrawRay(boss.transform.position, boss.dir * raycastDistancia, Color.red); 
        #endregion


        if (hitFrontal.collider != null)
        {
            Debug.Log("Obstáculo detectado, recalculando dirección...");
            boss.animatorToro.SetBool("IsCharging", false);

            boss.directionTime = 0; 
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
