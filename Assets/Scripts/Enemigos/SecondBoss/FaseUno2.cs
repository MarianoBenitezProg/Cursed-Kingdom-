using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseUno2 : SecondBossState
{
    private int step = 0; 

    public void EnterState(SecondBoss boss)
    {
        Debug.Log("? Entrando a la Fase Uno...");

        if (boss.Points1.Count >= 4)
        {
            boss.transform.position = boss.Points1[0].transform.position; 
            step = 1; 
        }

    }

    public void UpdateState(SecondBoss boss)
    {
        if (boss.Points1.Count < 4) return; 

        boss.transform.position = Vector2.MoveTowards(boss.transform.position,GetTargetPosition(boss),boss.speed * Time.deltaTime);

        if (Vector2.Distance(boss.transform.position, GetTargetPosition(boss)) < 0.1f)
        {
            HandleStepChange();
        }
    }

    private void HandleStepChange()
    {
        step = (step + 1) % 4;
    }

    private Vector2 GetTargetPosition(SecondBoss boss)
    {
        return boss.Points1[step].transform.position;
    }

    public void ExitState(SecondBoss boss)
    {
        Debug.Log("?? Saliendo de la Fase Uno...");
    }
}
