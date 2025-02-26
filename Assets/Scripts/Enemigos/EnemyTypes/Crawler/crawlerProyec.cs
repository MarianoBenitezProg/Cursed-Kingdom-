using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crawlerProyec : Proyectile
{
    private float Lifetimer;
    public int dmg;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 3)
        {
            var p_behaviour = other.GetComponent<P_Behaviour>();
            if (p_behaviour != null)
            {

                p_behaviour.Stunned(2f, 0, true);
                p_behaviour.TakeDamage(dmg);

                ProyectilePool.Instance.ReturnObstacle(gameObject, ProjectileType.CrawlerAttack);
            }
        }
    }
    private void Update()
    {
        Lifetimer += Time.deltaTime;
        if (Lifetimer > 5f) 
        {
            ProyectilePool.Instance.ReturnObstacle(gameObject, ProjectileType.CrawlerAttack);

        }
    }
}