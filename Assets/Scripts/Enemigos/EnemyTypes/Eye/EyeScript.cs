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

            // Verificar si hay un obstáculo en la dirección de retroceso
            if (HasObstacleInDirection(retreatDirection))
            {
                // Si hay un obstáculo, elegir una dirección alternativa (izquierda o derecha)
                retreatDirection = ChooseAlternativeDirection(retreatDirection);
            }

            // Mover al enemigo en la dirección elegida
            transform.position += retreatDirection * speed * Time.deltaTime;
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
        // Lanzar un Raycast para detectar obstáculos en la dirección dada
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, 1 << 6); // Layer 6 es la pared
        return hit.collider != null;
    }

    private Vector3 ChooseAlternativeDirection(Vector3 originalDirection)
    {
        // Intentar moverse a la derecha
        Vector3 rightDirection = new Vector3(-originalDirection.y, originalDirection.x, 0).normalized;
        if (!HasObstacleInDirection(rightDirection))
        {
            return rightDirection;
        }

        // Intentar moverse a la izquierda
        Vector3 leftDirection = new Vector3(originalDirection.y, -originalDirection.x, 0).normalized;
        if (!HasObstacleInDirection(leftDirection))
        {
            return leftDirection;
        }

        // Si no hay direcciones disponibles, mantener la dirección original
        return originalDirection;
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