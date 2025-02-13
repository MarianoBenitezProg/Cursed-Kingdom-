using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mar_Damage : Ability
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6) //Obstacle/Enemy Layer, so that if for any reason it touches the player it wont hit it
        {
            int damageDealt = dmg;
            if (P_Manager.Instance.isMarkusBuff == true)
            {
                Debug.Log(P_Manager.Instance.isMarkusBuff);
                damageDealt *= damageBuff;
            }
            Debug.Log("Pego");
            ItakeDamage takeDamage = collision.gameObject.GetComponent<ItakeDamage>();
            if (takeDamage != null)
            {
                takeDamage.TakeDamage(damageDealt);
            }
            timer = 0;
            ProyectilePool.Instance.ReturnObstacle(this.gameObject, ProyectType);
        }
        if (collision.gameObject.layer == 9)
        {
            timer = 0;
            ProyectilePool.Instance.ReturnObstacle(this.gameObject, ProyectType);
        }
    }
}
