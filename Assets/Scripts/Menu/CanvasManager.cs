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
    public bool isGamePaused;

    [Header("Dialogue")]
    public Text dialogueText;


    private void Update()
    {
        if (isGameCanvas == true && isGamePaused == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                isGamePaused = true;
                Time.timeScale = 0;
                ScreenManager.Instance?.Push("Game Menu");
            }
        }
        else if(isGameCanvas == true && isGamePaused == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                isGamePaused = false;
                Time.timeScale = 1;
                ScreenManager.Instance?.Pop();
            }
        }
    }

    public void BTN_LoadLevelScene(int selectedSlot)
    {
        if (selectedSlot == 1)
        {
            SavedGameManager.instance.selectedSaveSlot = SaveSlot.SlotOne;
            LevelsManager.instance?.LoadScene(SavedGameManager.instance.saveSlots[selectedSlot].level);
        }
        else if (selectedSlot == 2)
        {
            SavedGameManager.instance.selectedSaveSlot = SaveSlot.SlotTwo;
            LevelsManager.instance?.LoadScene(SavedGameManager.instance.saveSlots[selectedSlot].level);
        }
        else if (selectedSlot == 3)
        {
            SavedGameManager.instance.selectedSaveSlot = SaveSlot.SlotThree;
            LevelsManager.instance?.LoadScene(SavedGameManager.instance.saveSlots[selectedSlot].level);
        }

         // Load Levels Scene
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1;
    }

    public void BTN_BackToMenu()
    {
        SoundBTN_Pressed();
        isGamePaused = false;
        Debug.Log("Testing");
        LevelsManager.instance?.LoadScene("Main Menu"); // Load Menu Scene
    }

    public void BTN_CallScreen(string screenName)
    {
        SoundBTN_Pressed();
        ScreenManager.Instance?.Push(screenName);
    }
    public void BTN_Back()
    {
        SoundBTN_Pressed();
        isGamePaused = false;
        ScreenManager.Instance.Pop();
    }
    public void BTN_ExitGame()
    {
       Application.Quit();
        Debug.Log("Quit");
    }

    public void SoundBTN_Enter()
    {
        SoundManager.instance?.PlaySound("Ui Touch");
    }
    public void SoundBTN_Pressed()
    {
        SoundManager.instance?.PlaySound("Ui Close");
    }

    public void BTN_ResetSaveSlot(int saveSlot)
    {
        SoundBTN_Pressed();
        if(saveSlot == 1)
        {
            SavedGameManager.instance?.RestoreData(SaveSlot.SlotOne);
        }
        if (saveSlot == 2)
        {
            SavedGameManager.instance?.RestoreData(SaveSlot.SlotTwo);
        }
        if (saveSlot == 3)
        {
            SavedGameManager.instance?.RestoreData(SaveSlot.SlotThree);
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
