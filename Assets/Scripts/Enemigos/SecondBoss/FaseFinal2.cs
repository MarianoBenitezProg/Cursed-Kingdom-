using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseFinal2 : SecondBossState
{
    public void EnterState(SecondBoss boss)
    {

        boss.transform.position = boss.spawnPoints[1].transform.position;
        boss.speed += 2;
        boss.damage += 2;


        spawnearEnemigos(boss);
    }

    #region spawnEnemys 
    void spawnearEnemigos(SecondBoss boss)
    {


        GameObject enemigo1 = GameObject.Instantiate(boss.enemigsToSpawn[2], boss.spawnPoints[0].transform.position, Quaternion.identity);
        GameObject enemigo2 = GameObject.Instantiate(boss.enemigsToSpawn[2], boss.spawnPoints[0].transform.position, Quaternion.identity);
        GameObject enemigo3 = GameObject.Instantiate(boss.enemigsToSpawn[2], boss.spawnPoints[1].transform.position, Quaternion.identity);
        GameObject enemigo4 = GameObject.Instantiate(boss.enemigsToSpawn[2], boss.spawnPoints[1].transform.position, Quaternion.identity);

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
        Move(boss);

    }
    private void Move(SecondBoss boss)
    {



    }
    public void ExitState(SecondBoss boss)
    {
        Debug.Log("Saliendo de la Fase Final...");
    }
}