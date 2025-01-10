using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    protected bool isActive;
    public float speed;
    protected float timer;
    public int dmg;
    public float destroyTimer = 5f;
    public ProjectileType ProyectType;


    private void Update()
    {
        Behaviour();
    }
    public virtual void Behaviour()
    {

    }
}