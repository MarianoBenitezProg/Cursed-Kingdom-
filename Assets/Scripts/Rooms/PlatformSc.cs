using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSc : MonoBehaviour,ItakeDamage
{
    public int health = 30;
    public int Currenthealth;

    private void Awake()
    {
        Currenthealth = health;
    }
    private void Update()
    {

        if(Currenthealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void TakeDamage(int dmg)
    {
        if(Currenthealth > 0)
        {
            Currenthealth -= dmg;
        }
    }

}
