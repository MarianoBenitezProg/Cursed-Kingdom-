using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fer_CC : Ability
{
    public override void Behaviour()
    {
        if (isActive == true)
        {
            ShootingDirection();
            timer += Time.deltaTime;
        }

        if (timer >= destroyTimer)
        {
            ProyectilePool.Instance.ReturnObstacle(this.gameObject, ProyectType);
            isActive = false;
            timer = 0;
        }
    }
}
