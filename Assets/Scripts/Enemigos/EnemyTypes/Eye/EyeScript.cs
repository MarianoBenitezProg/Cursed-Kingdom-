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
    public int indexToGo = 0; 

    public override void Attack()
    {
        if (player == null) return;

        directionToPlayer = (player.transform.position - transform.position).normalized;
        angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        lookingDir = GetLookDirection(player.transform.position, this.transform.position);
        UpdateAnimation();

        if (playerDist < attackRadius - 1 && playerDist > 1f && path.Count > 0)
        {
            int midIndex = path.Count / 2; 
            int lastIndex = path.Count ; 

            if (indexToGo == 0 )
            {
                transform.position = Vector3.MoveTowards(transform.position, path[midIndex].position, speed * Time.deltaTime);
                if(Vector3.Distance(transform.position, path[midIndex].position) < 5f)
                {
                    indexToGo++;
                }
            }
            if (indexToGo == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, path[0].position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, path[0].position) < 5f)
                {
                    indexToGo = 0;
                }
            }
        }

        timer += Time.deltaTime;
        if (timer >= shootTimer && isDying == false)
        {
            Shoot();
            timer = 0f;
        }
    }

    private bool HasObstacleInDirection(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, 1 << 6); 
        return hit.collider != null;
    }


    public override void Seek()
    {
        timer += Time.deltaTime;
        lookingDir = GetLookDirection(player.transform.position, this.transform.position);
        UpdateAnimation();
        if (timer >= shootTimer && isDying == false)
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

        if (_tintMaterial != null)
        {
            _tintMaterial.SetTintColor(Color.red);
            Debug.Log("Tint");
        }
    }
}