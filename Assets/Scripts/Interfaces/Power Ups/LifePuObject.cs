using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePuObject : PickUps, IPickUp
{
    public void SavePickUp()
    {
        M_Inventory.instance.lifePowerUp = true;
    }
}
