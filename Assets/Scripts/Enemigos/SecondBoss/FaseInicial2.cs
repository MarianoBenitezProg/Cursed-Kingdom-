using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseInicial2 : SecondBossState
{
    public void EnterState(SecondBoss boss)
    {
        Debug.Log("Entrando a la Fase Inicial ...");

    }
    public void UpdateState(SecondBoss boss)
    {

    }
    public void ExitState(SecondBoss boss)
    {
        Debug.Log("Saliendo de la Fase Inicial ...");
    }
}