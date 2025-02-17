using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Level
{
    public string name;
    public GameObject levelGO;
}

public class LevelsManager : MonoBehaviour
{
    public static LevelsManager instance;

    [SerializeField] Dictionary<string, GameObject> levelsDictionary = new Dictionary<string, GameObject>();
    public Level[] levels;
    public string currentLevelName;
    public GameObject currentLevelGO;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize sounds from inspector
        foreach (Level lvl in levels)
        {
            levelsDictionary.Add(lvl.name, lvl.levelGO);
        }

        if (SavedGameManager.instance != null)
        {
            for (int i = 0; i < SavedGameManager.instance.saveSlots.Count; i++)
            {
                if (SavedGameManager.instance.selectedSaveSlot == SavedGameManager.instance.saveSlots[i].slot)
                {
                    if(levelsDictionary.ContainsKey(SavedGameManager.instance.saveSlots[i].level))
                    {
                        GameObject levelPrefab = levelsDictionary[SavedGameManager.instance.saveSlots[i].level];
                        currentLevelGO = Instantiate(levelPrefab);
                    }
                }
            }
        }
    }
    private void OnDestroy()
    {
        SaveData();
    }

    public void SaveData()
    {
        if (SavedGameManager.instance != null)
        {
            for (int i = 0; i < SavedGameManager.instance.saveSlots.Count; i++)
            {
                if (SavedGameManager.instance.selectedSaveSlot == SavedGameManager.instance.saveSlots[i].slot)
                {
                    SavedGameData UpdateLifeData = SavedGameManager.instance.saveSlots[i]; //You can´t just change the Life from the save slot, you gotta change the whole SaveSlot
                    UpdateLifeData.level = currentLevelName;
                    SavedGameManager.instance.saveSlots[i] = UpdateLifeData;
                }
            }
        }
    }

    public void ChangeLevel(string newLevel)
    {
        Destroy(currentLevelGO);
        if (levelsDictionary.ContainsKey(newLevel))
        {
            GameObject levelPrefab = levelsDictionary[newLevel];
            currentLevelGO = Instantiate(levelPrefab);
        }
    }
}
