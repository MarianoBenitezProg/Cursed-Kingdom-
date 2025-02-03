using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        }
        else if (selectedSlot == 2)
        {
            SavedGameManager.instance.selectedSaveSlot = SaveSlot.SlotTwo;
        }
        else if (selectedSlot == 3)
        {
            SavedGameManager.instance.selectedSaveSlot = SaveSlot.SlotThree;
        }

        StartCoroutine(LoadSceneAsync(1)); // Load Levels Scene
    }

    public void BTN_BackToMenu()
    {
        StartCoroutine(LoadSceneAsync(0)); // Load Menu Scene
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

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        // Optional: Activate loading screen
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        // Start async loading
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // Prevent scene activation until loading is complete
        asyncLoad.allowSceneActivation = false;

        timer += Time.deltaTime;

        // Wait until loading progress reaches 0.9 (90%)
        while (!asyncLoad.isDone)
        {
            // Update timer
            timer += Time.deltaTime;

            // Calculate loading progress (normalized)
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // Update loading bar
            if (loadingBar != null)
                loadingBar.fillAmount = progress;

            // Allow scene activation after minimum load time and progress is complete
            if (asyncLoad.progress >= 0.9f && timer >= minimumLoadTime)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        // Optional: Deactivate loading screen
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }
}
