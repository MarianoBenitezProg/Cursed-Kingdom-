using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePU : MonoBehaviour, IPickableObject
{
    [SerializeField] ItemStored selectedItem;
    P_Behaviour playerRef;
    [SerializeField] int addLife;

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        Debug.Log(playerRef);
        if(playerRef != null)
        {
            playerRef.life += addLife;
            playerRef._view.TintCharacter(new Color(0, 1, 0, 1f));
        }
    }
}
