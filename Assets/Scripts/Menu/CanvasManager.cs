using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CanvasManager : MonoBehaviour, IScreen
{
    public GameObject loadingScreen;
    public Image loadingBar;
    public float minimumLoadTime = 1f;
    public float timer;
    public bool isGameCanvas;

    [Header("Dialogue")]
    public Text dialogueText;

    private void Update()
    {
        if (isGameCanvas == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                ScreenManager.Instance.Push("Game Menu");
            }
        }
    }

    public void BTN_LoadLevelScene(int selectedSlot)
    {
        if (selectedSlot == 1)
        {
            SavedGameManager.instance.selectedSaveSlot = SaveSlot.SlotOne;
            LevelsManager.instance.LoadScene(SavedGameManager.instance.saveSlots[1].level);
        }
        else if (selectedSlot == 2)
        {
            SavedGameManager.instance.selectedSaveSlot = SaveSlot.SlotTwo;
            LevelsManager.instance.LoadScene(SavedGameManager.instance.saveSlots[2].level);
        }
        else if (selectedSlot == 3)
        {
            SavedGameManager.instance.selectedSaveSlot = SaveSlot.SlotThree;
            LevelsManager.instance.LoadScene(SavedGameManager.instance.saveSlots[3].level);
        }

         // Load Levels Scene
    }

    public void BTN_BackToMenu()
    {
        LevelsManager.instance.LoadScene("Main Menu"); // Load Menu Scene
    }

    public void BTN_CallScreen(string screenName)
    {
        ScreenManager.Instance.Push(screenName);
    }
    public void BTN_Back()
    {
        ScreenManager.Instance.Pop();
    }
    public void BTN_ExitGame()
    {

    }

    public void BTN_ResetSaveSlot(int saveSlot)
    {
        if(saveSlot == 1)
        {
            SavedGameManager.instance.RestoreData(SaveSlot.SlotOne);
        }
        if (saveSlot == 2)
        {
            SavedGameManager.instance.RestoreData(SaveSlot.SlotTwo);
        }
        if (saveSlot == 3)
        {
            SavedGameManager.instance.RestoreData(SaveSlot.SlotThree);
        }
    }


    public void Activate()
    {
    }

    public void Deactivate()
    {
    }

    public void Free()
    {
        Destroy(gameObject);
    }
}
