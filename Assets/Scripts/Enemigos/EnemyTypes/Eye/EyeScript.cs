using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScript : Enemy, ItakeDamage
{
    float timer;
    public float shootTimer = 5f;
    float angleToPlayer;
    public Vector3 spawnPoint;
    Vector3 directionToPlayer;

    public override void Attack()
    {
        if (player == null) return;

        directionToPlayer = (player.transform.position - transform.position).normalized;
        angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        lookingDir = GetLookDirection(player.transform.position, this.transform.position);
        UpdateAnimation();

        if (playerDist < attackRadius - 1 && playerDist > 1f)
        {
            Vector3 retreatDirection = -(player.transform.position - transform.position).normalized;
            transform.position += retreatDirection * speed * Time.deltaTime;
        }


        timer += Time.deltaTime;
        if (timer >= shootTimer)
        {
            Shoot();
            timer = 0f;
        }
    }

    public override void Seek()
    {
        timer += Time.deltaTime;
        lookingDir = GetLookDirection(player.transform.position, this.transform.position);
        UpdateAnimation();
        if (timer >= shootTimer)
        {
            Shoot();
            timer = 0f;
        }
        if (player == null) return;
        directionToPlayer = (player.transform.position - transform.position).normalized;
        angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
    }
    private void Shoot()
    {
        spawnPoint = transform.GetChild(0).position;

        GameObject disparo = ProyectilePool.Instance.GetObstacle(ProjectileType.EyeEnemy);
        if (disparo != null)
        {
            disparo.transform.position = spawnPoint;
            disparo.transform.rotation = Quaternion.Euler(0f, 0f, angleToPlayer);
            Debug.Log("Disparo generado.");
        }
        else
        {
            Debug.LogWarning("No hay proyectiles disponibles en el pool.");
        }
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
    }
}
