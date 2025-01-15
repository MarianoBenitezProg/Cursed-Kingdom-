using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyePROYEC : Proyectile
{
    private float time;
    private float Lifetimer;

    public float amplitude = 0.01f;
    public override void Behaviour()
    {
        Lifetimer += Time.deltaTime;    
        if(Lifetimer >= destroyTimer)
        {
            ProyectilePool.Instance.ReturnObstacle(gameObject, ProjectileType.EyeEnemy);
            Lifetimer = 0;
        }

        if (gameObject.activeSelf)
        {
            transform.position += transform.right * speed * Time.deltaTime;
            time += Time.deltaTime;

        }


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var check = other.gameObject.GetComponent<ItakeDamage>();
        if (check != null)
        {
            check.TakeDamage(dmg);
        }else
        {
            Debug.Log("no se le puede hacer daño ");
        }
    }
}
