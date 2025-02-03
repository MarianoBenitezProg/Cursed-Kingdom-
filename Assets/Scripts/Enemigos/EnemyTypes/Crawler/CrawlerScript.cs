using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerScript : Enemy, ItakeDamage
{
    int dmg = 15;
    float Atacktimer;
    float CoolDowntimer;
    bool needToCoolDown;

    public override void Seek()
    {
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public override void Attack()
    {
        var playerScript = player.GetComponent<P_Behaviour>();

        if (playerScript != null)
        {
            if (needToCoolDown == false)
            {
                playerScript.Stunned(3f, 0f, true);
                needToCoolDown = true;
            }

            if (needToCoolDown == true)
                CoolDowntimer += Time.deltaTime;

            if (CoolDowntimer >= 5f)
            {
                needToCoolDown = false;
                CoolDowntimer = 0;
            }

            Atacktimer += Time.deltaTime;

            if (Atacktimer >= 3f)
            {
                playerScript.TakeDamage(dmg);
                Atacktimer = 0f;
            }
        }

    }




    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
    }
}
