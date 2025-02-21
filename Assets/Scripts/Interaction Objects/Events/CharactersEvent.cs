using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharactersEvent : MonoBehaviour
{
    public P_Behaviour player;
    public float distanceToAction;

    public abstract void Action();
}
