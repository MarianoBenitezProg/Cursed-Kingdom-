using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseDos : BossState
{
    FirstBoss bossScript;
    private float moveTimer;
    private int currentStep;
    private float basicShootTimer;
    private float warningTimer;
    private bool warningPlaced;
    private Vector3 warningPosition;

    private bool waitingAtOriginalPos;
    private float waitTimer;

    public void EnterState(FirstBoss boss)
    {
        bossScript = boss;
        Debug.Log("Entré en la Fase dos");

        // Desactivo el colider
        boss.colider.enabled = false;

        foreach (GameObject plataforma in boss.Plataformas)
        {
            var vidaPlataforma = plataforma.GetComponent<PlatformSc>();
            vidaPlataforma.Currenthealth = vidaPlataforma.health;
            plataforma.SetActive(false); // Apagar plataformas
        }

        // Activar las plataformas necesarias
        boss.Plataformas[0].SetActive(true);
        boss.Plataformas[5].SetActive(true);

        // Reiniciar variables
        moveTimer = 0f;
        currentStep = 0;
        basicShootTimer = 0f;
        warningTimer = 0f;
        warningPlaced = false;
        waitingAtOriginalPos = false;
        waitTimer = 0f;

        boss.transform.position = boss.Plataformas[0].transform.position;
    }

    public void UpdateState(FirstBoss boss)
    {
        basicShootTimer += Time.deltaTime;
        warningTimer += Time.deltaTime;

        // Si el jefe está en modo de vulnerabilidad
        if (waitingAtOriginalPos)
        {
            boss.colider.enabled = true;
            waitTimer += Time.deltaTime;

            if (waitTimer >= 2f)
            {
                ReactivatePlatforms(boss);
                waitingAtOriginalPos = false;
                waitTimer = 0f;
            }
            return;
        }

        // Si no hay plataformas activas, el jefe vuelve a su posición original
        if (!boss.Plataformas[0].activeSelf && !boss.Plataformas[5].activeSelf)
        {
            boss.transform.position = boss.originalPos;
            waitingAtOriginalPos = true;
            waitTimer = 0f;
            return;
        }

        moveTimer += Time.deltaTime;
        if (moveTimer >= 1.5f)
        {
            MoveInVPattern(boss);
            moveTimer = 0f;
        }

        // Disparo básico cada 1.5 segundos
        if (basicShootTimer >= 1.5f)
        {
            BasicShot(boss);
            bossScript._animator.SetTrigger("IsAttacking");
            basicShootTimer = 0f;
        }

        // Ataque secundario con advertencia cada 4 segundos
        if (warningTimer >= 4f)
        {
            PlaceWarning(boss);
            warningTimer = 0f; // Reinicio el timer para que vuelva a ejecutarse en 4s
            warningPlaced = false; // Permitimos que el proyectil se vuelva a generar
        }
    }

    private void MoveInVPattern(FirstBoss boss)
    {
        switch (currentStep)
        {
            case 0:
                boss.transform.position = boss.originalPos;
                currentStep = 1;
                break;
            case 1:
                if (boss.Plataformas[5].activeInHierarchy)
                {
                    boss.transform.position = boss.Plataformas[5].transform.position;
                }
                else
                {
                    boss.transform.position = boss.originalPos;
                }
                currentStep = 2;
                break;
            case 2:
                boss.transform.position = boss.originalPos;
                currentStep = 3;
                break;
            case 3:
                if (boss.Plataformas[0].activeInHierarchy)
                {
                    boss.transform.position = boss.Plataformas[0].transform.position;
                }
                else
                {
                    boss.transform.position = boss.originalPos;
                }
                currentStep = 0;
                break;
        }
    }

    private void BasicShot(FirstBoss boss)
    {
        if (boss.player == null)
        {
            return;
        }

        Vector3 direction = (boss.player.transform.position - boss.ShootPoint).normalized;
        float angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject disparo = ProyectilePool.Instance.GetObstacle(ProjectileType.EyeEnemy);
        if (disparo != null)
        {
            disparo.transform.position = boss.ShootPoint;
            disparo.transform.rotation = Quaternion.Euler(0f, 0f, angleToPlayer);
            disparo.SetActive(true);
        }
        else
        {
            Debug.LogError("No se pudo obtener el proyectil de la pool.");
        }
    }

    private void PlaceWarning(FirstBoss boss)
    {
        if (boss.player == null)
        {
            Debug.LogWarning("No se encontró al jugador, no se puede colocar el proyectil.");
            return;
        }

        // Posición del jugador con un offset para que aparezca arriba
        warningPosition = boss.player.transform.position + new Vector3(0f, 1.5f, 0f);

        GameObject disparo = ProyectilePool.Instance.GetObstacle(ProjectileType.PlantAtack);

        if (disparo != null)
        {
            disparo.transform.position = warningPosition;
            disparo.SetActive(true); // Asegurar que el proyectil está activo en la escena
            Debug.Log("Proyectil colocado en " + warningPosition);
        }
        else
        {
            Debug.LogError("No se pudo obtener el proyectil de la pool.");
        }
    }

    private void ReactivatePlatforms(FirstBoss boss)
    {
        var vidaPlataforma2 = boss.Plataformas[0].GetComponent<PlatformSc>();
        var vidaPlataforma5 = boss.Plataformas[5].GetComponent<PlatformSc>();

        vidaPlataforma2.Currenthealth = vidaPlataforma2.health;
        vidaPlataforma5.Currenthealth = vidaPlataforma5.health;

        boss.Plataformas[0].SetActive(true);
        boss.Plataformas[5].SetActive(true);
    }

    public void ExitState(FirstBoss boss)
    {
        Debug.Log("Saliendo de la Fase Dos...");
    }
}
