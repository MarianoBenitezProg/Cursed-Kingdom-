using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RataScript : Enemy
{

    float Atacktimer;
    int dmg = 5;
    float angleToPlayer;
    Vector3 directionToPlayer;
    public override void Attack()
    {
        if (player == null) return;
        if(playerDist >1.2)
        {
        Vector3 FowardDirection = (player.transform.position - transform.position).normalized;
        transform.position += FowardDirection * speed * Time.deltaTime;
        }else if(playerDist < 1.2)
        {

            var playersc = player.GetComponent<P_Behaviour>();
            if(playersc != null)
            {
                Atacktimer += Time.deltaTime;
                if(Atacktimer >= 1.5)
                {
                    playersc.TakeDamage(dmg);
                    Atacktimer = 0;
                }
            }
        }


    }


    public override void Seek()
    {
        if (player == null) return;
        directionToPlayer = (player.transform.position - transform.position).normalized;
        angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angleToPlayer);
    }
}
