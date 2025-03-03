using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseUno2 : SecondBossState
{
    Vector3 puntoRush;
    public void EnterState(SecondBoss boss)
    {
        boss.rb.velocity = Vector2.zero;
        boss.transform.position =  boss.originPoint;
        boss.speed += 2;
        boss.damage += 2;

        spawnearEnemigos(boss);
    }


    #region spawnEnemys
    void spawnearEnemigos(SecondBoss boss)
    {

        GameObject enemigo1 = GameObject.Instantiate(boss.enemigsToSpawn[0], boss.spawnPoints[0].transform.position, Quaternion.identity);
        GameObject enemigo2 = GameObject.Instantiate(boss.enemigsToSpawn[0], boss.spawnPoints[0].transform.position, Quaternion.identity);
        GameObject enemigo3 = GameObject.Instantiate(boss.enemigsToSpawn[0], boss.spawnPoints[1].transform.position, Quaternion.identity);
        GameObject enemigo4 = GameObject.Instantiate(boss.enemigsToSpawn[0], boss.spawnPoints[1].transform.position, Quaternion.identity);

        // Asignación de paths únicos para cada enemigo
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
        if (boss.player != null)
        {
            boss.dir = (boss.player.transform.position - boss.transform.position).normalized;
            boss.transform.rotation = Quaternion.LookRotation(Vector3.forward, boss.dir);
            puntoRush = boss.transform.position;

        }
    }

    private void Move(SecondBoss boss)
    {
        float distanciaRecorrida = Vector3.Distance(puntoRush, boss.transform.position);

        float raycastDistancia = 1f; 
        RaycastHit2D hitFrontal = Physics2D.Raycast(boss.transform.position, boss.dir, raycastDistancia, boss.obstacleLayer);

        Debug.DrawRay(boss.transform.position, boss.dir * raycastDistancia, Color.red); // Dibuja el Raycast en la escena


        if (hitFrontal.collider != null)
        {
            Debug.Log("Obstáculo detectado, recalculando dirección...");
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
