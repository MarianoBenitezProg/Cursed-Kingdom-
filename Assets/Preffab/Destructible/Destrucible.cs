using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destrucible : MonoBehaviour, ItakeDamage
{
    public int life;

    public void takeDMG(int dmg)
    {
        life -= dmg;
    }
}
