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
        spawnPoint = transform.GetChild(0).position;
        if (player != null && enemyHasSight)
        {

            if (needToAtack)
            {
                Vector3 retreatDirection = -(player.transform.position - transform.position).normalized;
                transform.position += retreatDirection * speed * Time.deltaTime;
            }

            directionToPlayer = player.transform.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            timer += Time.deltaTime;
            if (timer >= shootTimer)
            {
                GameObject disparo = ProyectilePool.Instance.GetObstacle(ProjectileType.EyeEnemy);
                if (disparo != null)
                {
                    disparo.transform.position = spawnPoint;
                    disparo.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }
                timer = 0f;
            }
        }
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
    }
}
