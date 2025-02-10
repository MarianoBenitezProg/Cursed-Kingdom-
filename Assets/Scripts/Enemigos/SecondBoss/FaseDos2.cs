using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseDos2 : SecondBossState
{
    private int step = 0; 
    private bool reverse = false; 

    public void EnterState(SecondBoss boss)
    {
        Debug.Log(" Entrando a la Fase Dos...");

        boss.transform.position = boss.Points2[0].transform.position; 
        step = 1; 
        reverse = false;
    }

    public void UpdateState(SecondBoss boss)
    {

        Vector3 targetPosition = boss.Points2[step].transform.position;
        boss.transform.position = Vector2.MoveTowards(boss.transform.position,targetPosition,boss.speed * Time.deltaTime);

        if (Vector2.Distance(boss.transform.position, targetPosition) < 0.1f)
        {
            HandleStepChange(boss);
        }
    }

    private void HandleStepChange(SecondBoss boss)
    {
        if (!reverse)
        {
            step++;
            if (step >= boss.Points2.Count - 1) reverse = true;
        }
        else
        {
            step--; 
            if (step <= 0) reverse = false; 
        }

    }

    public void ExitState(SecondBoss boss)
    {
        Debug.Log("Saliendo de la Fase Dos...");
    }
}
