using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseFinal : BossState
{
    private int movimientos; // Controla el tipo de movimiento
    private int currentEspIndex = 0; // Índice para espiral
    private int currentVIndex = 0; // Índice para movimiento en "V"

    private float moveTimer = 0f;
    private float moveCooldown = 1.5f; // ? Controla la velocidad de los movimientos

    public void EnterState(FirstBoss boss)
    {
        Debug.Log("? Entré en la Fase Final");
        boss.colider.enabled = false;

        // ? Activa todas las plataformas y resetea su vida
        foreach (GameObject plataforma in boss.Plataformas)
        {
            var vidaPlataforma = plataforma.GetComponent<PlatformSc>();
            vidaPlataforma.Currenthealth = vidaPlataforma.health;
            plataforma.SetActive(true);
        }

        movimientos = 0;
        currentEspIndex = 0;
        currentVIndex = 0;
    }

    public void UpdateState(FirstBoss boss)
    {
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
                    Debug.Log("?? Movimientos completos. Pasando al siguiente ataque.");
                    break;
            }
            moveTimer = 0f; // ?? Reinicia el timer después de cada movimiento
        }
    }

    #region **Movimientos**
    private void EspiralMovimiento(FirstBoss boss)
    {
        Debug.Log("? Movimiento en Espiral");
        List<GameObject> plataformas = boss.Plataformas;

        int startIndex = plataformas.Count;
        for (int i = 0; i < plataformas.Count; i++)
        {

            if (plataformas[i].activeSelf)
            {
                boss.transform.position = plataformas[i].transform.position;
                return; // ?? Se mueve y sale del método
            }

            // ? Si visitamos todas las plataformas activas, cambiamos de movimiento
            if (i >= startIndex)
            {
                Debug.Log("?? Fin del Espiral, cambiando al movimiento en V...");
                ApagarTodasLasPlataformas(boss);
                boss.Plataformas[2].SetActive(true);
                boss.Plataformas[5].SetActive(true);
                movimientos = 1;
                return;
            }
        }
    }

    private void MovimientoEnV(FirstBoss boss)
    {
        Debug.Log("? Movimiento en V");

        switch (currentVIndex)
        {
            case 0:
                boss.transform.position = boss.originalPos;
                currentVIndex = 1;
                break;

            case 1:
                if (boss.Plataformas[5].activeSelf)
                {
                    boss.transform.position = boss.Plataformas[5].transform.position;
                }
                else
                {
                    boss.transform.position = boss.originalPos;
                }
                currentVIndex = 2;
                break;

            case 2:
                boss.transform.position = boss.originalPos;
                currentVIndex = 3;
                break;

            case 3:
                if (boss.Plataformas[2].activeSelf)
                {
                    boss.transform.position = boss.Plataformas[2].transform.position;
                }
                else
                {
                    boss.transform.position = boss.originalPos;
                }

                // ? Apaga las plataformas y pasa al siguiente movimiento
                ApagarTodasLasPlataformas(boss);
                movimientos = 2;
                currentVIndex = 0;
                break;
        }
    }
    #endregion

    #region **Utilidades**
    private void ApagarTodasLasPlataformas(FirstBoss boss)
    {
        Debug.Log("?? Apagando todas las plataformas...");
        foreach (GameObject plataforma in boss.Plataformas)
        {
            plataforma.SetActive(false);
        }
    }
    #endregion

    public void ExitState(FirstBoss boss)
    {
        Debug.Log("Saliendo de la Fase Final...");
    }
}
