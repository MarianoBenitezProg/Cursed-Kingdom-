using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseUno : BossState
{
    private float pathTimer;
    private int currentIndex;
    private bool waitingAtOriginalPos;
    private float waitTimer;

    public  void EnterState(FirstBoss boss)
    {
        Debug.Log("Entré en la Fase Uno");
        pathTimer = 0f;
        waitTimer = 0f;
        waitingAtOriginalPos = false;
        currentIndex = 0;
        boss.colider.enabled = false;
    }

    public  void UpdateState(FirstBoss boss)
    {
        // Disparo cada 1.5 segundos
        boss.BasicShootTimer += Time.deltaTime;
        if (boss.BasicShootTimer > 1.5f)
        {
            BasicShot(boss);
            boss.BasicShootTimer = 0f;
        }

        // espera 5 segundos para recibir daño y ver si pasa a fase dos o sigue en fase uno 
        if (waitingAtOriginalPos)
        {
            boss.colider.enabled = true;

            waitTimer += Time.deltaTime;
            if (waitTimer >= 5f)
            {
                ReactivatePlatforms(boss);
                waitingAtOriginalPos = false;
                waitTimer = 0f;
            }
            return;
        }

        // Si todas las plataformas están inactivas, se vuelve al medio 
        if (AreAllPlatformsInactive(boss))
        {
            Debug.Log("Todas las plataformas están inactivas. Volviendo a la posición original...");
            boss.transform.position = boss.originalPos;
            waitingAtOriginalPos = true;
            waitTimer = 0f;
            return;
        }

        //se mueve  entre plataformas activas cada 2 segundos
        pathTimer += Time.deltaTime;
        if (pathTimer >= 2f)
        {
            if (!MoveToNextActivePlatform(boss))
            {
                Debug.Log("No hay plataformas activas, moviéndose a originalPos...");
                boss.transform.position = boss.originalPos;
                waitingAtOriginalPos = true;
                waitTimer = 0f;
            }
            pathTimer = 0f;
        }
    }

    private void BasicShot(FirstBoss boss)
    {
        if (boss.player == null)
        {
            Debug.LogWarning("No se ha asignado un jugador en FirstBoss.");
            return;
        }

        Vector3 direction = (boss.player.transform.position - boss.ShootPoint).normalized;
        float angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        
        GameObject disparo = ProyectilePool.Instance.GetObstacle(ProjectileType.EyeEnemy);
        if (disparo != null)
        {
            disparo.transform.position = boss.ShootPoint;
            disparo.transform.rotation = Quaternion.Euler(0f, 0f, angleToPlayer);
            Debug.Log("Disparo generado.");
        }
        else
        {
            Debug.LogWarning("No hay proyectiles disponibles en el pool.");
        }
    }

    private bool MoveToNextActivePlatform(FirstBoss boss)
    {
        int startIndex = currentIndex;
        for (int i = 0; i < boss.Plataformas.Count; i++)
        {
            currentIndex = (currentIndex + 1) % boss.Plataformas.Count;
            if (boss.Plataformas[currentIndex].activeSelf)
            {
                boss.transform.position = boss.Plataformas[currentIndex].transform.position;
                return true;
            }
            if (currentIndex == startIndex)
            {
                return false;
            }
        }
        return false;
    }

    private bool AreAllPlatformsInactive(FirstBoss boss)
    {
        foreach (GameObject plataforma in boss.Plataformas)
        {
            if (plataforma.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    private void ReactivatePlatforms(FirstBoss boss)
    {
        Debug.Log("Reactivando todas las plataformas...");
        foreach (GameObject plataforma in boss.Plataformas)
        {
            var vidaPlataforma = plataforma.GetComponent<PlatformSc>();
            vidaPlataforma.Currenthealth = vidaPlataforma.health;

            plataforma.SetActive(true);
        }
        boss.colider.enabled = false;

    }

    public  void ExitState(FirstBoss boss)
    {
        Debug.Log("Saliendo de la Fase Uno...");
    }
}
