using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    float timer;
    public float DestroyTimer = 5f;
    public ProjectileType ProyectType;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= DestroyTimer)
        {
            ProyectilePool.Instance.ReturnObstacle(gameObject, ProyectType);
            timer = 0;
        }
    }
    public virtual void Behaviour()
    {

    }
}