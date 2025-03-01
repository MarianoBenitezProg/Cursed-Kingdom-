using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToroProyec : Proyectile
{

    private float Lifetimer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 3)
        {
            var p_behaviour = other.GetComponent<P_Behaviour>();
            if (p_behaviour != null)
            {
                p_behaviour.TakeDamage(dmg);
            }
        }
    }
    private void Update()
    {
        Lifetimer += Time.deltaTime;
        if (Lifetimer > 5f)
        {
            ProyectilePool.Instance.ReturnObstacle(gameObject, ProjectileType.SecondBossAtack);

        }
    }
}
