using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : Enemy, ItakeDamage
{
    private float timer; 
    private bool warningPlaced; 
    private Vector3 warningPosition; 

    public GameObject warningGB; 

    public override void Attack()
    {
        timer += Time.deltaTime;

        if (!warningPlaced && timer >= 0f) 
        {
            warningPosition = player.transform.position;
            GameObject warningInstance = Instantiate(warningGB, warningPosition, Quaternion.identity); // Instancia del aviso
            Destroy(warningInstance, 3f);
            warningPlaced = true; 
        }

        if (warningPlaced && timer >= 3f)
        {
            GameObject attackProjectile = ProyectilePool.Instance.GetObstacle(ProjectileType.PlantAtack);
            if (attackProjectile != null)
            {
                attackProjectile.transform.position = warningPosition; 
                attackProjectile.transform.rotation = Quaternion.identity; 
                attackProjectile.SetActive(true);
            }
            timer = 0;
            warningPlaced = false;
        }
    }


    public override void Seek()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var abilty = collision.gameObject.GetComponent<Ability>();
        if (abilty != null)
        {
            TakeDamage(abilty.dmg);
        }
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
    }
}
