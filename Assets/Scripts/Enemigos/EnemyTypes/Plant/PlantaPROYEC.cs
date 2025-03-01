using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantaPROYEC : MonoBehaviour
{
    public CircleCollider2D colider;
    public float Lifetimer;
    public float destroyTimer;
    public float warningTimer;
    public int dmg;
    public ProjectileType ProyectType;

    private void Awake()
    {
        colider = GetComponent<CircleCollider2D>();
        colider.enabled = false;


    }

    private void Update()
    {
        warningTimer += Time.deltaTime;

        if (!colider.enabled && warningTimer >= 3f)
        {
            Debug.Log("Activando colider");
            colider.enabled = true;
            warningTimer = 0;
        }


        Lifetimer += Time.deltaTime;
        if (Lifetimer >= destroyTimer)
        {
            colider.enabled = false;
            ProyectilePool.Instance.ReturnObstacle(gameObject, ProyectType);
            Lifetimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var check = other.gameObject.GetComponent<ItakeDamage>();
        if (check != null)
        {
            check.TakeDamage(dmg);
            colider.enabled = false;
            ProyectilePool.Instance.ReturnObstacle(gameObject, ProyectType);
        }
        else
        {
            Debug.Log("No se le puede hacer daño");
        }
    }
}
