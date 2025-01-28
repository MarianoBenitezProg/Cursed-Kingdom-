using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantaPROYEC : Proyectile
{
    private float Lifetimer;

    public override void Behaviour()
    {
        Lifetimer += Time.deltaTime;
        if (Lifetimer >= destroyTimer)
        {
            ProyectilePool.Instance.ReturnObstacle(gameObject, ProjectileType.PlantAtack);
            Lifetimer = 0;
        }

        if (gameObject.activeSelf)
        {
            transform.position += transform.right * speed * Time.deltaTime;

        }


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var check = other.gameObject.GetComponent<ItakeDamage>();
        if (check != null)
        {
            check.TakeDamage(dmg);
            ProyectilePool.Instance.ReturnObstacle(gameObject, ProjectileType.PlantAtack);
        }
        else
        {
            Debug.Log("no se le puede hacer daño ");
        }
    }
}
