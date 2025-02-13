using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonShield : MonoBehaviour, ItakeDamage
{
    SpriteRenderer shieldRenderer;
    SkeletonScript skeletonScript;
    int health = 3;
    Direction currentDirection;
    MaterialTintColor _tintColor;
    private void Awake()
    {
        skeletonScript = GetComponentInParent<SkeletonScript>();
        currentDirection = skeletonScript.lookingDir;
        shieldRenderer = GetComponent<SpriteRenderer>();
        _tintColor = GetComponent<MaterialTintColor>();
    }
    private void Update()
    {
        if(currentDirection != skeletonScript.lookingDir)
        {
            if (skeletonScript.lookingDir == Direction.Up)
            {
                this.transform.position = new Vector3(skeletonScript.transform.position.x -.6f, skeletonScript.transform.position.y -.2f, 0);
                shieldRenderer.sortingOrder = -2;
            }
            if (skeletonScript.lookingDir == Direction.Down)
            {
                this.transform.position = new Vector3(skeletonScript.transform.position.x +.5f, skeletonScript.transform.position.y - .8f, 0);
                shieldRenderer.sortingOrder = 0;
            }
            if (skeletonScript.lookingDir == Direction.Left)
            {
                this.transform.position = new Vector3(skeletonScript.transform.position.x - 0.55f, skeletonScript.transform.position.y - 0.55f, 0);
                shieldRenderer.sortingOrder = 0;
            }
            if (skeletonScript.lookingDir == Direction.Right)
            {
                shieldRenderer.sortingOrder = 0;
                this.transform.position = new Vector3(skeletonScript.transform.position.x + .4f, skeletonScript.transform.position.y- .5f, 0);
            }
            currentDirection = skeletonScript.lookingDir;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var abilty = collision.gameObject.GetComponent<Ability>();
        if (abilty != null && abilty.isFerana == true)
        {
            TakeDamage(1);
            if (health <= 0)
            {
                skeletonScript.IsDamageable = true;
                skeletonScript.thisCollider.enabled = true;
                Destroy(gameObject);
            }
        }
        else if(abilty != null && abilty.isFerana == false)
        {
            _tintColor.SetTintColor(Color.grey);
        }
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        _tintColor.SetTintColor(new Color(1.0f, 0.64f, 0.0f));
        Debug.Log(health);
    }
}
