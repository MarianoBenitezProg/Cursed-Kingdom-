using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScript : Enemy, ItakeDamage
{
    float timer;
    public float shootTimer = 5f;
    public Vector3 spawnPoint;
    Vector3 directionToPlayer;

    public override void Attack()
    {
       if(player != null && enemyHasSight == true)
        {
            spawnPoint = gameObject.transform.GetChild(0).position;

            directionToPlayer = player.transform.position - transform.position;


            if(player != null && needToAtack == true)
            {

            Vector3 retreatDirection = -directionToPlayer.normalized;
            transform.position += retreatDirection * speed * Time.deltaTime;
            }

            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);


            timer += Time.deltaTime;
            if(timer >= shootTimer)
            {
              GameObject disparo =  ProyectilePool.Instance.GetObstacle(ProjectileType.EyeEnemy);

                disparo.transform.position = spawnPoint;

                float angleBullet = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                disparo.transform.rotation = Quaternion.Euler(0f, 0f, angleBullet);                
                timer = 0;
            }
        }
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
    }
}
