using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScript : Enemy
{
    public float timer;
    public override void Atack()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obstacle = ProyectilePool.Instance.GetObstacle(ProjectileType.Markus1);
            obstacle.transform.position = new Vector3(1,1,0);
        }

    }


}
