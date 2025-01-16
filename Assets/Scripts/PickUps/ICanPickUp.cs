using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ICanPickUp
{
    public void StoreObject(ItemStored pickedObject);
}