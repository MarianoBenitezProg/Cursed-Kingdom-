using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScript : Enemy
{
    public float timer;
    public float Shoottimer = 5f;
    public Vector3 spawnPoint;
    Vector3 directionToPlayer;
    public override void Atack()
    {
       if(player != null && enemyHasSight == true)
        {
            spawnPoint = gameObject.transform.GetChild(0).position;

            directionToPlayer = player.transform.position - transform.position;


            //ir para atras 
            Vector3 retreatDirection = -directionToPlayer.normalized;
            transform.position += retreatDirection * 1f * Time.deltaTime;

            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);


            timer += Time.deltaTime;
            if(timer >= Shoottimer)
            {
              GameObject disparo =  ProyectilePool.Instance.GetObstacle(ProjectileType.EyeEnemy);

                disparo.transform.position = spawnPoint;

                float angleBullet = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                disparo.transform.rotation = Quaternion.Euler(0f, 0f, angleBullet);                
                timer = 0;
            }
        }



    }


}
