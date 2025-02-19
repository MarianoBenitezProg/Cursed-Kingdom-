using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public string nextLevelName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            if(LevelsManager.instance != null)
            {
                for (int i = 0; i < SavedGameManager.instance.saveSlots.Count; i++)
                {
                    if (SavedGameManager.instance.selectedSaveSlot == SavedGameManager.instance.saveSlots[i].slot)
                    {
                        SavedGameData UpdateLifeData = SavedGameManager.instance.saveSlots[i]; //You can´t just change the Life from the save slot, you gotta change the whole SaveSlot
                        UpdateLifeData.level = nextLevelName;
                        SavedGameManager.instance.saveSlots[i] = UpdateLifeData;
                    }
                }
                LevelsManager.instance.LoadScene(nextLevelName);
            }
        }
    }
}
