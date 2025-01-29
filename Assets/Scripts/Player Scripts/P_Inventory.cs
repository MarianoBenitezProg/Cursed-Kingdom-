using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[System.Serializable]public struct ItemStored
{
    public PickUpType objectType;
    public bool isPicked;
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
                    HUD_Manager.instance.SetInventoryUI(pickedObject.objectType, true);
                    Debug.Log("<color=green>Item marked as picked</color>");
                }
                return;
            }
        }
        itemsList.Add(pickedObject);
        HUD_Manager.instance.SetInventoryUI(pickedObject.objectType, true);
        Debug.Log("<color=blue>New item added to list</color>");
    }

    public void RunEffect(PickUpType effectToRun, TypeEvent EventType)
    {
        for (int i = 0; i < itemsList.Count; i++)
        {
            if (itemsList[i].objectType == effectToRun && itemsList[i].isPicked == true )
            {
                ItemStored updatedItem = itemsList[i];
                updatedItem.isPicked = false;
                itemsList[i] = updatedItem;
                EventManager.Trigger(EventType);
                HUD_Manager.instance.SetInventoryUI(effectToRun, false);
                return;
            }
        }
    }

    public void SaveInventory()
    {
        for (int i = 0; i < SavedGameManager.instance.saveSlots.Count; i++)
        {
            if(SavedGameManager.instance.selectedSaveSlot == SavedGameManager.instance.saveSlots[i].slot)
            {
                for (int y = 0; y < SavedGameManager.instance.saveSlots[i].items.Length; y++)
                {
                    for (int z = 0; z < itemsList.Count; z++)
                    {
                        if(SavedGameManager.instance.saveSlots[i].items[y].objectType == itemsList[z].objectType)
                        {
                            SavedGameManager.instance.saveSlots[i].items[y] = itemsList[z];
                        }
                    }
                }
            }
        }
    }

    public void RestoreInventory()
    {
        //Here also i miss the conditions if its a new lvl, so the inventory should be empty.
        Debug.Log("Restoration Start");

        for (int i = 0; i < SavedGameManager.instance.saveSlots.Count; i++)
        {
            Debug.Log("Entro 1");
            if (SavedGameManager.instance.selectedSaveSlot == SavedGameManager.instance.saveSlots[i].slot)
            {
                Debug.Log("Entro 2");
                for (int y = 0; y < SavedGameManager.instance.saveSlots[i].items.Length; y++)
                {
                    Debug.Log("Entro 3");
                    if(SavedGameManager.instance.saveSlots[i].items[y].isPicked == true)
                    {
                        itemsList.Add(SavedGameManager.instance.saveSlots[i].items[y]);
                        HUD_Manager.instance.SetInventoryUI(SavedGameManager.instance.saveSlots[i].items[y].objectType, true);
                        Debug.Log("Item Restored" + SavedGameManager.instance.saveSlots[i].items[y].objectType);
                    }
                }
            }
        }
    }   
}