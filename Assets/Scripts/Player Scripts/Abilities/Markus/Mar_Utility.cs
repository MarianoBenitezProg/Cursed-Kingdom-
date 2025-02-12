using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mar_Utility : Ability
{
    public float originalSize;
    public float explotionSize = 2;
    public float explotionTimer;
    float originalSpeed;
    SpriteRenderer thisSprite;
    CircleCollider2D expCollider;
    [SerializeField] GameObject particlesPrefab;
    bool isExploding;
    float createParticles = 0;

    private void Start()
    {
        expCollider = GetComponent<CircleCollider2D>();
        expCollider.radius = originalSize;
        thisSprite = GetComponent<SpriteRenderer>();
        originalSpeed = speed;
    }

    public override void Behaviour()
    {
        if (isActive == true)
        {
            thisSprite.enabled = true;
            ShootingDirection();
            timer += Time.deltaTime;
        }

        if (timer >= destroyTimer)
        {
            expCollider.radius = explotionSize;
            isActive = false;
            isExploding = true;
            createParticles++;
            timer = 0;
        }
        if(isExploding == true)
        {
            if (createParticles == 1)
            {
                Instantiate(particlesPrefab,transform.position,Quaternion.identity);
                createParticles++;
            }
            speed = 0;
            explotionTimer += Time.deltaTime;
            if(explotionTimer >= 0.5)
            {
                expCollider.radius = originalSize;
                ProyectilePool.Instance.ReturnObstacle(this.gameObject, ProyectType);
                explotionTimer = 0;
                speed = originalSpeed;
                createParticles = 0;
                isExploding = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6) //Obstacle/Enemy Layer, so that if for any reason it touches the player it wont hit it
        {
            int damageDealt = dmg;
            if (P_Manager.Instance.isMarkusBuff == true)
            {
                damageDealt *= damageBuff;
            }
            isExploding = true;
            thisSprite.enabled = false;
            createParticles++;
            expCollider.radius = explotionSize;
            Debug.Log("Exploto");
            ItakeDamage takeDamage = collision.gameObject.GetComponent<ItakeDamage>();
            if (takeDamage != null)
            {
                takeDamage.TakeDamage(damageDealt);
            }
            timer = 0;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, expCollider.radius);
    }
}
