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
        if(enemigos.Count == 0)
        {
            puntoFinal.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 8)
        {
            var enemy = collision.GetComponent<Enemy>();
            var warden = collision.GetComponent<SecondBoss>();

            if (enemy != null)
            {
                enemigos.Add(collision.gameObject);
            }

            if (warden != null)
            {
                enemigos.Add(collision.gameObject);
            }
        }
    }
    public List<GameObject> GetEnemigos()
    {
        return enemigos;
    }
}

