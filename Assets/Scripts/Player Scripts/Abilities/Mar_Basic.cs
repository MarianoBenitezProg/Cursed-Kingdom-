using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mar_Basic : Proyectile
{
    private void Update()
    {
        if(isActive == true)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        timer += Time.deltaTime;
        if(timer >= destroyTimer)
        {
            ProyectilePool.Instance.ReturnObstacle(this.gameObject, ProyectType);
            isActive = false;
            timer = 0;
        }
    }
    
    public override void Behaviour()
    {
        base.Behaviour();
    }
}
