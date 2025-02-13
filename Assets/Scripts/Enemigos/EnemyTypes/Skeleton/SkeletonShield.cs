using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonShield : MonoBehaviour, ItakeDamage
{
    SkeletonScript skeletonScript;
    int health = 3;

    private void Awake()
    {
        skeletonScript = GetComponentInParent<SkeletonScript>();
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
                Destroy(gameObject);
            }
        }
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
    }
}
