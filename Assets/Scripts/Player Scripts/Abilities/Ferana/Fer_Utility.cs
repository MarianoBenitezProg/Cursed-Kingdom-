using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fer_Utility : Ability
{
    [SerializeField] float stunTime;

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
            if (P_Manager.Instance.isFeranaBuff == true)
            {
                damageDealt *= damageBuff;
            }
            Debug.Log("Pego");
            ItakeDamage takeDamage = collision.gameObject.GetComponent<ItakeDamage>();
            if (takeDamage != null)
            {
                takeDamage.TakeDamage(damageDealt);
                Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
                if(enemyScript != null)
                {
                    enemyScript.Stunned(stunTime, 3, false);
                }
            }
            timer = 0;
            ProyectilePool.Instance.ReturnObstacle(this.gameObject, ProyectType);
        }
        if(collision.gameObject.layer == 10)
        {
            timer = 0;
            ProyectilePool.Instance.ReturnObstacle(this.gameObject, ProyectType);
        }
    }
}
