using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseFinal2 : SecondBossState
{
    public void EnterState(SecondBoss boss)
    {
        Debug.Log("Entrando a la Fase Final...");

        boss.transform.position = boss.originPoint;

    }
    public void UpdateState(SecondBoss boss)
    {

    }
    public void ExitState(SecondBoss boss)
    {
        Debug.Log("Saliendo de la Fase Final...");
    }
}