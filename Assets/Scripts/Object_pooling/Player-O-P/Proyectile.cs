using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    float timer;
    public ProjectileType ProyectType;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5)
        {
            ProyectilePool.Instance.ReturnObstacle(gameObject, ProyectType);
        }
    }
    public virtual void Behaviour()
    {

    }
}