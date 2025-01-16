using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUps
{
    public PickUpType _pickUpType;
}

[System.Serializable] public enum PickUpType
{
    Life,
    FeranaPowerUp,
    MarkusPowerUp
}
