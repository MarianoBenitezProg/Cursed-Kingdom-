using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : Enemy, ItakeDamage
{
    private float timer; 
    private bool warningPlaced; 
    private Vector3 warningPosition;
    public float warningTime;
    float cooldownTimer;

    public GameObject warningGB; 
    public override void Attack()
    {
        cooldownTimer += Time.deltaTime;
        if (!warningPlaced && cooldownTimer >= 1f) 
        {
            _animator.SetBool("IsCharging",true);
            warningPosition = player.transform.position;
            GameObject warningInstance = Instantiate(warningGB, warningPosition, Quaternion.identity); // Instancia del aviso
            Destroy(warningInstance, warningTime);
            warningPlaced = true; 
        }
        if(warningPlaced == true)
        {
            timer += Time.deltaTime;
        }

        if (warningPlaced && timer >= warningTime && isDying == false)
        {
            GameObject attackProjectile = ProyectilePool.Instance.GetObstacle(ProjectileType.PlantAtack);
            
            if (attackProjectile != null)
            {
                attackProjectile.transform.position = warningPosition; 
                attackProjectile.transform.rotation = Quaternion.identity; 
                attackProjectile.SetActive(true);
            }
            _animator.SetBool("IsCharging", false);
            timer = 0;
            cooldownTimer = 0;
            warningPlaced = false;
        }

        if (isDying == true)
        {
            _animator.SetBool("IsDying", true);
        }
    }

    public override void Seek()
    {
        _animator.SetBool("IsAwake", true);
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
        _tintMaterial.SetTintColor(Color.red);
    }
}
