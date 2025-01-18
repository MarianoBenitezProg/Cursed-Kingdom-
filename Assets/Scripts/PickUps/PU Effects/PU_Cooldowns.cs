using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_Cooldowns : MonoBehaviour, IPickableObject
{

    [SerializeField] ItemStored selectedItem;
    AbilityTimers abilityScript;
    P_Behaviour playerRef;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        abilityScript = collision.gameObject.GetComponent<AbilityTimers>();
        playerRef = collision.gameObject.GetComponent<P_Behaviour>();
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
        playerRef._view.TintCharacter(new Color(0, 0.48f, 1, 1f)); ;
    }
}
