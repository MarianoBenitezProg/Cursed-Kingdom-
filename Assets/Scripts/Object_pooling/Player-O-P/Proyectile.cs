using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    protected bool isActive;
    protected float timer;
    public float speed;
    public float destroyTimer = 5f;
    public ProjectileType ProyectType;
    protected GameObject shootingPivot;

    private void Awake()
    {
        shootingPivot = GameObject.Find("Shooting Pivot");
    }
    private void OnEnable()
    {
        transform.position = shootingPivot.transform.position;
        isActive = true;
    }

    //private void Update()
    //{
    //    timer += Time.deltaTime;
    //    if (timer >= DestroyTimer)
    //    {
    //        ProyectilePool.Instance.ReturnObstacle(gameObject, ProyectType);
    //        timer = 0;
    //    }
    //}
    public virtual void Behaviour()
    {

    }
}