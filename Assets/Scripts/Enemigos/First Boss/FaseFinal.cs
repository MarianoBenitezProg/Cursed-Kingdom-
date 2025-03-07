using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseFinal : BossState
{
    private int movimientos; // Controla el tipo de movimiento
    private int currentEspIndex = 0; // �ndice para espiral
    private int currentVIndex = 0; // �ndice para movimiento en "V"
    private int currentRAndomIndex = 0; // �ndice para movimiento en "Random"
    private GameObject plataformaAnterior = null;


    private float basicShootTimer;
    private float warningTimer;
    private bool warningPlaced;
    private Vector3 warningPosition;


    private float moveTimer = 0f;
    private float moveCooldown = 1.5f; // ? Controla la velocidad de los movimientos

    public void EnterState(FirstBoss boss)
    {
        Debug.Log("? Entr� en la Fase Final");
        boss.colider.enabled = false;

        // ? Activa todas las plataformas y resetea su vida
        foreach (GameObject plataforma in boss.Plataformas)
        {
            var vidaPlataforma = plataforma.GetComponent<PlatformSc>();
            vidaPlataforma.Currenthealth = vidaPlataforma.health;
            BoxCollider2D platformCol =  plataforma.GetComponent<BoxCollider2D>();
            platformCol.enabled = false;
            plataforma.SetActive(true);

        }

        movimientos = 0;
        currentEspIndex = 0;
        currentVIndex = 0;
    }

    public void UpdateState(FirstBoss boss)
    {
        // ? Actualiza los temporizadores de disparo
        basicShootTimer += Time.deltaTime;
        warningTimer += Time.deltaTime;

        // ?? Disparo b�sico cada 1.5 segundos
        if (basicShootTimer >= 1.5f)
        {
            BasicShot(boss);
            boss._animator.SetTrigger("IsAttacking");
            basicShootTimer = 0f; // ?? Reinicia el timer
        }

        // ?? Coloca la advertencia cada 4 segundos
        if (warningTimer >= 4f)
        {
            PlaceWarning(boss);
            warningTimer = 0f; // Reinicio el timer para que vuelva a ejecutarse en 4s
            warningPlaced = false; // Permitimos que el proyectil se vuelva a generar
        }


        // ? Actualiza el temporizador de movimiento
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveCooldown)
        {
            switch (movimientos)
            {
                case 0:
                    EspiralMovimiento(boss);
                    break;
                case 1:
                    MovimientoEnV(boss);
                    break;
                case 2:
                    MovimientoRandom(boss);
                    break;
                case 3:
                    boss.colider.enabled = true;
                    break;
            }
            moveTimer = 0f;
        }
    }
    private void EspiralMovimiento(FirstBoss boss)
    {
        Debug.Log(" Movimiento en Espiral");

        if (currentEspIndex < boss.Plataformas.Count)
        {
            boss.transform.position = boss.Plataformas[currentEspIndex].transform.position;
            Debug.Log($"?? Movi�ndose a plataforma {currentEspIndex}");
            currentEspIndex++;
        }

        if (currentEspIndex >= boss.Plataformas.Count)
        {
            movimientos = 1;
        }
    }

    private void MovimientoEnV(FirstBoss boss)
    {

        Debug.Log("? Movimiento en V");

        foreach (GameObject plataforma in boss.Plataformas)
        {
            var vidaPlataforma = plataforma.GetComponent<PlatformSc>();
            vidaPlataforma.Currenthealth = vidaPlataforma.health;
            plataforma.SetActive(false);
        }
        // prendo las que necesito 
        boss.Plataformas[0].SetActive(true);
        boss.Plataformas[5].SetActive(true);

        switch (currentVIndex)
        {
            case 0:
                boss.transform.position = boss.Plataformas[0].transform.position;
                Debug.Log(" Movi�ndose a Plataforma[2]");
                currentVIndex = 1;
                break;

            case 1:
                boss.transform.position = boss.originalPos;
                Debug.Log(" Movi�ndose a OriginalPos");
                currentVIndex = 2;
                break;

            case 2:
                boss.transform.position = boss.Plataformas[5].transform.position;
                Debug.Log(" Movi�ndose a Plataforma[5]");
                currentVIndex = 3;
                break;

            case 3:
                boss.transform.position = boss.originalPos;
                Debug.Log(" Movi�ndose a OriginalPos");
                currentVIndex = 4;
                break;

            case 4:
                boss.transform.position = boss.Plataformas[0].transform.position;
                Debug.Log(" Movi�ndose a Plataforma[2]");
                currentVIndex = 5;
                break;

            case 5:
                boss.transform.position = boss.originalPos;
                foreach (GameObject plataforma in boss.Plataformas)
                {
                    var vidaPlataforma = plataforma.GetComponent<PlatformSc>();
                    vidaPlataforma.Currenthealth = vidaPlataforma.health;
                    plataforma.SetActive(true);
                }
                movimientos = 2; 
                currentVIndex = 0; 
                break;
        }
    }

    private void MovimientoRandom(FirstBoss boss)
    {
        Debug.Log("? Movimiento Aleatorio");

        GameObject nuevaPlataforma = null;


        if (TodasLasPlataformasDesactivadas(boss))
        {
            boss.transform.position = boss.originalPos;
            movimientos = 3;
            return;
        }

        for (int i = 0; i < boss.Plataformas.Count; i++)
        {
            int randomNum = Random.Range(0, boss.Plataformas.Count); 
            if (boss.Plataformas[randomNum].activeSelf)
            {
                nuevaPlataforma = boss.Plataformas[randomNum];
                break; 
            }
        }

        if (nuevaPlataforma == null) return;

        if (plataformaAnterior != null)
        {
            plataformaAnterior.SetActive(false); 
        }

        boss.transform.position = nuevaPlataforma.transform.position;
        Debug.Log($"?? Movi�ndose a plataforma {boss.Plataformas.IndexOf(nuevaPlataforma)}");

        plataformaAnterior = nuevaPlataforma;

    }

    private bool TodasLasPlataformasDesactivadas(FirstBoss boss)
    {
        foreach (GameObject plataforma in boss.Plataformas)
        {
            if (plataforma.activeSelf) return false;
        }
        return true;
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
            Debug.LogWarning("No se encontr� al jugador, no se puede colocar el proyectil.");
            return;
        }

        // Posici�n del jugador con un offset para que aparezca arriba
        warningPosition = boss.player.transform.position + new Vector3(0f, 1.5f, 0f);

        GameObject disparo = ProyectilePool.Instance.GetObstacle(ProjectileType.PlantAtack);

        if (disparo != null)
        {
            disparo.transform.position = warningPosition;
            disparo.SetActive(true); // Asegurar que el proyectil est� activo en la escena
            PlantaPROYEC proyectilScript = disparo.GetComponent<PlantaPROYEC>();
            proyectilScript.colider.enabled = false; // Desactiva la colisi�n al spawnear
            proyectilScript.Lifetimer = 0f; // Reinicia su tiempo de vida
            proyectilScript.warningTimer = 0f;
            Debug.Log("Proyectil colocado en " + warningPosition);
        }
        else
        {
            Debug.LogError("No se pudo obtener el proyectil de la pool.");
        }
    }


    public void ExitState(FirstBoss boss)
    {
        Debug.Log("Saliendo de la Fase Final...");
    }
}
