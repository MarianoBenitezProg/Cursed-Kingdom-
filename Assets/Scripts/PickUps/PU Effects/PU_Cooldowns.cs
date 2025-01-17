using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_Cooldowns : MonoBehaviour, IPickableObject
{

    [SerializeField] ItemStored selectedItem;
    AbilityTimers abilityScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        abilityScript = collision.gameObject.GetComponent<AbilityTimers>();
        ICanPickUp obj = collision.gameObject.GetComponent<ICanPickUp>();
        if (obj != null)
        {
            ObjectPicked(obj);
        }
    }

    public void ObjectPicked(ICanPickUp obj)
    {
        selectedItem.isPicked = true;
        selectedItem.storedEffect = PU_Effect;
        obj.StoreObject(selectedItem);
        Destroy(gameObject);
    }

    public void PU_Effect()
    {
        abilityScript.ResetAllCooldowns();
    }
}
