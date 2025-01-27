using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BreakableObjects : MonoBehaviour
{
    [SerializeField]protected int objectLife;
    protected abstract void Destruction();
}
