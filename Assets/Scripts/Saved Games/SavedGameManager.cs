using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public struct SavedGameData
{
    public SaveSlot slot;
    public int level;
    public int checkpoint;
    public int life;
    public ItemStored[] items;
}
[System.Serializable] public enum SaveSlot
{
    SlotOne,
    SlotTwo,
    SlotThree
}

public class SavedGameManager : MonoBehaviour
{
    public List<SavedGameData> saveSlots= new List<SavedGameData>();

    public static SavedGameManager instance;
    public SaveSlot selectedSaveSlot;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
