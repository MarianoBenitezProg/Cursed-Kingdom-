using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseInicial2 : SecondBossState
{
    public void EnterState(SecondBoss boss)
    {
        Debug.Log("estoy en la fase inicial");
    }
    public void UpdateState(SecondBoss boss)
    {
        Move(boss);

    }

    private void Move(SecondBoss boss)
    {
        if (boss.player != null && boss.playerDistance > 6)
        {
            boss.animatorToro.SetBool("IsWalking", true);
            boss.animatorToro.SetBool("IsAttacking", false);

            boss.dir = (boss.player.transform.position - boss.transform.position).normalized;
            boss.rb.velocity = boss.dir * boss.speed;
        }
        else if(boss.player != null &&  boss.playerDistance < 6)
        {
            boss.animatorToro.SetBool("IsAttacking", true);
            boss.animatorToro.SetBool("IsWalking", true);

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
        Debug.Log("estoy shoteando");
        var proyetile = ProyectilePool.Instance.GetObstacle(ProjectileType.SecondBossAtack);
        proyetile.transform.position = boss.transform.position + boss.dir * 5;

        var toroProyec = proyetile.GetComponent<ToroProyec>();
        toroProyec.dmg = boss.damage;
    }

    public void ExitState(SecondBoss boss)
    {
        Debug.Log("Saliendo de la Fase Inicial ...");
    }
}