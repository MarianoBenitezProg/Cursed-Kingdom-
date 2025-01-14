using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mar_CC : Ability
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
            Debug.Log("Pego");
            ItakeDamage takeDamage = collision.gameObject.GetComponent<ItakeDamage>();
            if (takeDamage != null)
            {
                takeDamage.TakeDamage(dmg);
                Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
                enemyScript.Stunned(stunTime, 0, true);
            }
            ProyectilePool.Instance.ReturnObstacle(this.gameObject, ProyectType);
        }
    }
}
