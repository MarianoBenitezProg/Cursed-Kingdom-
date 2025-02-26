using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerScript : Enemy, ItakeDamage
{
    int dmg = 15;
    float Atacktimer;
    public float attackCooldown = 3;

    public GameObject ataque;

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
            Atacktimer += Time.deltaTime;

            if (Atacktimer >= attackCooldown)
            {
                Atacktimer = 0f;

            GameObject ataque = ProyectilePool.Instance.GetObstacle(ProjectileType.CrawlerAttack);
            ataque.transform.position = player.transform.position;
            }
            return;
        

    }



    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
    }
}
