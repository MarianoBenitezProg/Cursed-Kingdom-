using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RataScript : Enemy, ItakeDamage
{

    float Atacktimer;
    int dmg = 5;
    float angleToPlayer;
    Vector3 directionToPlayer;
    public SpriteRenderer spriteRenderer;

    public override void Attack()
    {

        if(lookingDir == Direction.Right)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        lookingDir = GetLookDirection(player.transform.position, this.transform.position);
        UpdateAnimation();
        if (player == null) return;
        if(playerDist >1.2)
        {
        Vector3 FowardDirection = (player.transform.position - transform.position).normalized;
        transform.position += FowardDirection * speed * Time.deltaTime;
        }else if(playerDist < 1.2)
        {

            var playersc = player.GetComponent<P_Behaviour>();
            if(playersc != null)
            {
                Atacktimer += Time.deltaTime;
                if(Atacktimer >= 1.5)
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
        if (player == null) return;
        directionToPlayer = (player.transform.position - transform.position).normalized;
        angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angleToPlayer);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
        if(_tintMaterial != null)
        {
            _tintMaterial.SetTintColor(new Color(1, 0, 0, 1f));
            Debug.Log("TintColor");
        }
        if(health <= 0)
        {
            SoundManager.instance?.PlaySound("Mutant Dead");
        }
    }
}
