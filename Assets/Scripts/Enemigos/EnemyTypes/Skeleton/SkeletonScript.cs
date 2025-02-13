using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : Enemy,ItakeDamage
{
    float Atacktimer;
    int dmg = 20;
    public bool IsDamageable;
    GameObject escudo;
    public CapsuleCollider2D thisCollider;
    private void Start()
    {
        thisCollider = GetComponent<CapsuleCollider2D>();
        thisCollider.enabled = false;
    }
    public override void Attack()
    {
        lookingDir = GetLookDirection(player.transform.position, this.transform.position);
        UpdateAnimation();
            

        if (playerDist > 2)
        {
            Vector3 FowardDirection = (player.transform.position - transform.position).normalized;
            transform.position += FowardDirection * speed * Time.deltaTime;
        }else if (playerDist < 2)
        {

            var playersc = player.GetComponent<P_Behaviour>();
            if (playersc != null)
            {
                Atacktimer += Time.deltaTime;
                if (Atacktimer >= 1.5)
                {
                    playersc.TakeDamage(dmg);
                    Atacktimer = 0;
                }
            }
        }
    }

    public override void Seek()
    {
        lookingDir = GetLookDirection(player.transform.position, this.transform.position);
        UpdateAnimation();
        if (playerDist > 1 )
        {   
            
            Vector3 FowardDirection = (player.transform.position - transform.position).normalized;
            transform.position += FowardDirection * speed * Time.deltaTime;
        }
    }

    public void TakeDamage(int dmg)
    {
        if(IsDamageable == true)
        {
            health -= dmg;
            Debug.Log(health);
            _tintMaterial.SetTintColor(Color.red);
        }
    }
}
