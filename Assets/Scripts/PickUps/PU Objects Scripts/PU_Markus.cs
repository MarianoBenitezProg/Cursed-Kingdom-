using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_Markus : MonoBehaviour, IPickableObject
{
    [SerializeField] ItemStored selectedItem;
    [SerializeField] int buffTime;

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
        SoundManager.instance.PlaySound("FerMarkPU", 0.3f);
        gameObject.SetActive(false);
    }

}
