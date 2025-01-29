using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_Cooldowns : MonoBehaviour, IPickableObject
{

    [SerializeField] ItemStored selectedItem;
  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICanPickUp obj = collision.gameObject.GetComponent<ICanPickUp>();
        if (obj != null)
        {
            ObjectPicked(obj);
        }
    }

    public void ObjectPicked(ICanPickUp obj)
    {
        selectedItem.isPicked = true;
        obj.StoreObject(selectedItem);
        gameObject.SetActive(false);
    }

    
}
