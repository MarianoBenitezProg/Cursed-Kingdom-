using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Inventory : MonoBehaviour
{
    public static M_Inventory instance;
    [SerializeField] P_Behaviour _playerRef;

    LifePuEffect _lifePu;

    public bool lifePowerUp;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _lifePu = new LifePuEffect(_playerRef);
    }
}