using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interaction : MonoBehaviour
{
    [SerializeField]protected bool IsMarkus;
    [SerializeField] protected P_Behaviour _playerRef;
    [SerializeField] protected float _distanceForAction;
    [SerializeField] protected float _distanceWithPlayer;
    protected KeyCode interactionKey = KeyCode.E;

    public abstract void Action();
}
