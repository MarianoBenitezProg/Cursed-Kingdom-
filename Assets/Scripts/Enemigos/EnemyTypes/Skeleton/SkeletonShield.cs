using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonShield : MonoBehaviour, ItakeDamage
{
    int health = 3;

    private void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var abilty = collision.gameObject.GetComponent<Ability>();
        if (abilty != null && abilty.isFerana == true)
        {
            TakeDamage(1);
        }
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
    }
}
