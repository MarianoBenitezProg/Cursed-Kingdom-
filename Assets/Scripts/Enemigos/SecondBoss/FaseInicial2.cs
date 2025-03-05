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

        if (boss.cabezeo == true)
        {
            boss.animatorToro.SetBool("IsAttacking", false);

        }

    }

    private void Move(SecondBoss boss)
    {
        if (boss.player != null && boss.playerDistance > 3)
        {
            boss.animatorToro.SetBool("IsCharging", false);
            boss.animatorToro.SetBool("IsAttacking", false);
            boss.animatorToro.SetBool("IsWalking", true);

            boss.dir = (boss.player.transform.position - boss.transform.position).normalized;
            boss.rb.velocity = boss.dir * boss.speed;
        }
        else if(boss.player != null &&  boss.playerDistance < 3 )
        {
            boss.animatorToro.SetBool("IsWalking", false);
            boss.rb.velocity = Vector3.zero;
            boss.atackTimer += Time.deltaTime;

            if(boss.atackTimer >= boss.atackCountDown && boss.cabezeo == false)
            {
                shot(boss);
                boss.atackTimer = 0;

            }
            else if(boss.atackTimer >= boss.atackCountDown - 1f)
            {
                boss.animatorToro.SetBool("IsAttacking", true);

                Debug.Log("no puedo atacar");
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