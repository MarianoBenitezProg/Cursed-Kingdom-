using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseInicial : BossState
{
    public void EnterState(FirstBoss boss)
    {
        Debug.Log("entre en el la fase inicial");
        foreach (GameObject plataforma in boss.Plataformas)
        {
            var coliderPlataforma = plataforma.GetComponent<BoxCollider2D>();
            coliderPlataforma.enabled = false;
        }
    }
    public void UpdateState(FirstBoss boss)
    {

    }
    public void ExitState(FirstBoss boss)
    {
        foreach (GameObject plataforma in boss.Plataformas)
        {
            var coliderPlataforma = plataforma.GetComponent<BoxCollider2D>();
            coliderPlataforma.enabled = true;
        }

    }
}
