using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntoFinal : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemigos = new List<GameObject>();
    public GameObject puntoFinal;

    private void Awake()
    {
        puntoFinal = gameObject.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        // Limpiar lista de enemigos nulos o destruidos
        enemigos.RemoveAll(enemigo => enemigo == null);

        if (enemigos.Count == 0)
        {
            puntoFinal.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si está en las capas de enemigos
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 8)
        {
            // Verificar cada tipo de enemigo específico
            Component[] enemyComponents = {
                collision.gameObject.GetComponent<SkeletonScript>(),
                collision.gameObject.GetComponent<RataScript>(),
                collision.gameObject.GetComponent<CrawlerScript>(),
                collision.gameObject.GetComponent<EyeScript>(),
                collision.gameObject.GetComponent<SecondBoss>()
            };

            // Agregar a la lista si tiene alguno de los componentes
            foreach (var component in enemyComponents)
            {
                if (component != null)
                {
                    enemigos.Add(collision.gameObject);
                    break; // Salir después de agregar para evitar duplicados
                }
            }
        }
    }
}