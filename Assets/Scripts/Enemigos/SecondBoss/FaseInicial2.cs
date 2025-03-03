using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseInicial2 : SecondBossState
{
    public void EnterState(SecondBoss boss)
    {
        boss.transform.position = boss.originPoint;
        boss.speed += 2;
        boss.damage += 2;
        Debug.Log("estoy en la fase inicia");
    }
    public void UpdateState(SecondBoss boss)
    {
        Move(boss);

    }

    private void Move(SecondBoss boss)
    {
        // que se mueva hacia el player
        if (boss.player != null && boss.playerDistance > 6)
        {
            boss.dir = (boss.player.transform.position - boss.transform.position).normalized;
            boss.rb.velocity = boss.dir * boss.speed;

        }
        else
        {
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
        var proyetile = ProyectilePool.Instance.GetObstacle(ProjectileType.SecondBossAtack);
        var toroProyec = proyetile.GetComponent<ToroProyec>();
        toroProyec.dmg = boss.damage;
        proyetile.transform.position = boss.transform.position + boss.dir * 5;
    }

    public void ExitState(SecondBoss boss)
    {
        Debug.Log("Saliendo de la Fase Inicial ...");
    }
}