using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable] public enum PickUpType
{
    Life,
    MarkusPU,
    FeranaPU,
    CooldownReset
}

[System.Serializable]public struct ItemStored
{
    public PickUpType objectType;
    public bool isPicked;
    public Action storedEffect;
}

public class P_Inventory
{
    public List<ItemStored> itemsList = new List<ItemStored>();

    public P_Inventory()
    {
    }

    public void StoreItem(ItemStored pickedObject)
    {
        for (int i = 0; i < itemsList.Count; i++)
        {
            if(itemsList[i].objectType == pickedObject.objectType)
            {
                if(!itemsList[i].isPicked)
                {
                    ItemStored updatedItem = itemsList[i];
                    updatedItem.isPicked = true;
                    itemsList[i] = updatedItem;
                    Debug.Log($"StoredEffect is {(itemsList[i].storedEffect == null ? "null" : "valid")}");
                    Debug.Log("<color=green>Item marked as picked</color>");
                }
                return;
            }
        }
        itemsList.Add(pickedObject);
        Debug.Log("<color=blue>New item added to list</color>");
    }

    public void RunEffect(PickUpType effectToRun)
    {
        for (int i = 0; i < itemsList.Count; i++)
        {
            if (itemsList[i].objectType == effectToRun && itemsList[i].isPicked == true )
            {
                Debug.Log($"Running effect for item {i}, storedEffect is {(itemsList[i].storedEffect == null ? "null" : "valid")}");
                if (itemsList[i].storedEffect != null)
                {
                    itemsList[i].storedEffect();
                    ItemStored updatedItem = itemsList[i];
                    updatedItem.isPicked = false;
                    itemsList[i] = updatedItem;
                    return;
                }
            }
        }
    }
}