using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectLvl : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image loadingBar;
    public float minimumLoadTime = 1f;
    public float timer;

    public void BTN_LoadLvlScene(int selectedSlot)
    {
        if(selectedSlot == 0)
        {
            SavedGameManager.instance.selectedSaveSlot = SaveSlot.SlotOne;
        }
        else if(selectedSlot == 1)
        {
            SavedGameManager.instance.selectedSaveSlot = SaveSlot.SlotTwo;
        }
        else if (selectedSlot == 2)
        {
            SavedGameManager.instance.selectedSaveSlot = SaveSlot.SlotThree;
        }
        StartCoroutine(LoadSceneAsync(1)); // Load Levels Scene
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
