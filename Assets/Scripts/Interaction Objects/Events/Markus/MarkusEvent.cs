using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MarkusEvent : MonoBehaviour
{
    public P_Behaviour player;
    public float distanceToAction;

    public abstract void Action();
}
