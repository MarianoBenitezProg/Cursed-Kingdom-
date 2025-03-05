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
        // Limpia enemigos nulos
        enemigos.RemoveAll(enemigo => enemigo == null);

        if (enemigos.Count == 0)
        {
            puntoFinal.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica capas de enemigos
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 8)
        {
            // Detecta tipos específicos de enemigos
            if (collision.GetComponent<SkeletonScript>() != null ||
                collision.GetComponent<RataScript>() != null ||
                collision.GetComponent<CrawlerScript>() != null ||
                collision.GetComponent<EyeScript>() != null ||
                collision.GetComponent<SecondBoss>() != null)
            {
                enemigos.Add(collision.gameObject);
            }
        }
    }
}