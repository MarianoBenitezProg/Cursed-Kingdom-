using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseDos : BossState
{
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
        Debug.Log("Entré en la Fase dos");
        //desactivo el colider
        boss.colider.enabled = false;

        foreach (GameObject plataforma in boss.Plataformas)
        {
            //restauro la vida de las plataformas
            var vidaPlataforma = plataforma.GetComponent<PlatformSc>();
            vidaPlataforma.Currenthealth = vidaPlataforma.health;
            // las apago 
            plataforma.SetActive(false);
        }
        // prendo las que necesito 
        boss.Plataformas[2].SetActive(true);
        boss.Plataformas[5].SetActive(true);

        //variables de movimiento y disparos 
        moveTimer = 0f;
        currentStep = 0;
        basicShootTimer = 0f;
        warningTimer = 0f;
        warningPlaced = false;
        waitingAtOriginalPos = false;
        waitTimer = 0f;

        boss.transform.position = boss.Plataformas[2].transform.position;
    }

    public void UpdateState(FirstBoss boss)
    {
        basicShootTimer += Time.deltaTime;
        warningTimer += Time.deltaTime;

        //bool de vulnerabilidad
        if (waitingAtOriginalPos)
        {
            //activo el colider y me dejo pegar
            boss.colider.enabled = true;
            waitTimer += Time.deltaTime;

            //tiempo de vulnerabilidad
            if (waitTimer >= 2f)
            {
                ReactivatePlatforms(boss);
                waitingAtOriginalPos = false;
                waitTimer = 0f;
            }

            return;
        }

        //si no tengo plataformas
        if (!boss.Plataformas[2].activeSelf && !boss.Plataformas[5].activeSelf)
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
            basicShootTimer = 0f;
        }

        //Ataque secundario con advertencia cada 4 segundos
        if (!warningPlaced && warningTimer >= 4f)
        {
            PlaceWarning(boss);
        }

        if (warningPlaced && warningTimer >= 7f)
        {
            FireFromWarning(boss);
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
                if (boss.Plataformas[5].active == true)
                {
                boss.transform.position = boss.Plataformas[5].transform.position;
                }else
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

                if (boss.Plataformas[2].active == true)
                {
                    boss.transform.position = boss.Plataformas[2].transform.position;
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
        }

    }

    private void PlaceWarning(FirstBoss boss)
    {
        if (boss.player == null)
        {
            return;
        }

        warningPosition = boss.player.transform.position;
        GameObject warningInstance = GameObject.Instantiate(boss.warningGB, warningPosition, Quaternion.identity);
        GameObject.Destroy(warningInstance, 3f);

        warningPlaced = true;
    }

    private void FireFromWarning(FirstBoss boss)
    {
        GameObject attackProjectile = ProyectilePool.Instance.GetObstacle(ProjectileType.PlantAtack);
        if (attackProjectile != null)
        {
            attackProjectile.transform.position = warningPosition;
            attackProjectile.transform.rotation = Quaternion.identity;
            attackProjectile.SetActive(true);
        }


        warningTimer = 0f;
        warningPlaced = false;
    }

    private void ReactivatePlatforms(FirstBoss boss)
    {
        var vidaPlataforma2 = boss.Plataformas[2].GetComponent<PlatformSc>();
        var vidaPlataforma5 = boss.Plataformas[5].GetComponent<PlatformSc>();

        vidaPlataforma2.Currenthealth = vidaPlataforma2.health;
        vidaPlataforma5.Currenthealth = vidaPlataforma5.health;

        boss.Plataformas[2].SetActive(true);
        boss.Plataformas[5].SetActive(true);

    }

    public void ExitState(FirstBoss boss)
    {
        Debug.Log("Saliendo de la Fase Dos...");
    }
}
