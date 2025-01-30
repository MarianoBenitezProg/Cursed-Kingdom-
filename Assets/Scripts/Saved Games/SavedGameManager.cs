
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    Default,
    SlotOne,
    SlotTwo,
    SlotThree
}

public class SavedGameManager : MonoBehaviour
{
    public List<SavedGameData> saveSlots = new List<SavedGameData>();
    public static SavedGameManager instance;
    public SaveSlot selectedSaveSlot;

    private const string SAVE_KEY = "GameSaveData";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            LoadSaveData();
        }
        else
        {
            Destroy(gameObject);
        }
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
    }

    // Called when the game is closed normally
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    // Optional: Save periodically as backup
    private void Start()
    {
        // Start auto-save coroutine
        StartCoroutine(AutoSaveRoutine());
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(300f); // Auto-save every 5 minutes
            SaveGame();
            Debug.Log("Auto-saved game data");
        }
    }

    public void SaveGame()
    {
        try
        {
            // Convert the save slots list to JSON
            string saveData = JsonUtility.ToJson(new SaveDataWrapper { saves = saveSlots });

            // Save to PlayerPrefs
            PlayerPrefs.SetString(SAVE_KEY, saveData);
            PlayerPrefs.Save();

            Debug.Log("Game data saved successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save game data: {e.Message}");
        }
    }

    private void LoadSaveData()
    {
        try
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                string saveData = PlayerPrefs.GetString(SAVE_KEY);
                SaveDataWrapper wrapper = JsonUtility.FromJson<SaveDataWrapper>(saveData);

                if (wrapper != null && wrapper.saves != null && wrapper.saves.Count > 0)
                {
                    saveSlots = wrapper.saves;
                    Debug.Log("Game data loaded successfully");
                }
                else
                {
                    Debug.LogWarning("No valid save data found. Keeping initial save slots.");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load game data: {e.Message}");
        }
    }

    // Get save data for a specific slot
    public SavedGameData? GetSaveData(SaveSlot slot)
    {
        SavedGameData? save = saveSlots.FirstOrDefault(x => x.slot == slot);
        return save;
    }

    // Delete save data from a specific slot
    public void RestoreData(SaveSlot slot)
    {
        // Find the default save data
        SavedGameData? defaultData = saveSlots.FirstOrDefault(x => x.slot == SaveSlot.Default);

        if (defaultData.HasValue)
        {
            // Create a new instance with the same values (deep copy)
            SavedGameData newSaveData = new SavedGameData
            {
                slot = slot,
                level = defaultData.Value.level,
                checkpoint = defaultData.Value.checkpoint,
                life = defaultData.Value.life,
                items = defaultData.Value.items != null ? defaultData.Value.items.ToArray() : new ItemStored[0] // Ensure a proper copy of items
            };

            // Replace the target slot's data
            for (int i = 0; i < saveSlots.Count; i++)
            {
                if (saveSlots[i].slot == slot)
                {
                    saveSlots[i] = newSaveData;
                    SaveGame(); // Persist changes
                    return;
                }
            }

            Debug.LogWarning($"RestoreData: Slot {slot} not found in saveSlots.");
        }
        else
        {
            Debug.LogError("RestoreData: Default slot not found in saveSlots.");
        }
    }
}

// Wrapper class needed for JSON serialization of List
[System.Serializable]
public class SaveDataWrapper
{
    public List<SavedGameData> saves;
}
