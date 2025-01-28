using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public struct SavedGameData
{
    [SerializeField] SaveSlot slot;
    [SerializeField] int currentLevel;
    [SerializeField] int currentCheckpoint;
    [SerializeField] int currentLife;
    [SerializeField] int currentItems;
}
[System.Serializable] public enum SaveSlot
{
    SlotOne,
    SlotTwo,
    SlotThree
}

public class SavedGameManager : MonoBehaviour
{
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
