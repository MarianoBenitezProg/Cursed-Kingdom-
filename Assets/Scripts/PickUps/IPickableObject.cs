using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickableObject
{
    public void ObjectPicked(ICanPickUp obj);
}
