using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : Enemy,ItakeDamage
{
    float Atacktimer;
    int dmg = 20;
    bool IsDamageable;
    GameObject escudo;

    public override void Attack()
    {
        if(escudo != null)
        {
        escudo = gameObject.transform.GetChild(0).gameObject;
        }else
        {
            IsDamageable = true;
        }
      
           
        

        if (playerDist > 2)
        {
            Vector3 FowardDirection = (player.transform.position - transform.position).normalized;
            transform.position += FowardDirection * speed * Time.deltaTime;
        }else if (playerDist < 2)
        {

            var playersc = player.GetComponent<P_Behaviour>();
            if (playersc != null)
            {
                Atacktimer += Time.deltaTime;
                if (Atacktimer >= 1.5)
                {
                    playersc.TakeDamage(dmg);
                    Atacktimer = 0;
                }
            }
        }
    }

    public override void Seek()
    {
        if (playerDist > 1 )
        {
            Vector3 FowardDirection = (player.transform.position - transform.position).normalized;
            transform.position += FowardDirection * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var abilty = collision.gameObject.GetComponent<Ability>();
        if (abilty != null && IsDamageable == true)
        {
            TakeDamage(abilty.dmg);
        }
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
    }
}
