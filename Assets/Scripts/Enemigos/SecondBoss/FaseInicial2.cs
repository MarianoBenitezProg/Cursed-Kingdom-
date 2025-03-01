using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseInicial2 : SecondBossState
{
    public void EnterState(SecondBoss boss)
    {
        boss.transform.position = boss.spawnPoints[0].transform.position;
        boss.speed += 2;
        boss.damage += 2;
        boss.needToStop = false;
        boss.stopTimer = 0;
    }

    public void UpdateState(SecondBoss boss)
    {
        
    }

    public void ExitState(SecondBoss boss)
    {
        Debug.Log("Saliendo de la Fase Inicial ...");
    }
}