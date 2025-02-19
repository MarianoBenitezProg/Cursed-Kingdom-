using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public static LevelsManager instance;
    public string currentLevelName;
    float timer;
    float minimumLoadTime;
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
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }


    private IEnumerator LoadSceneAsync(string sceneName)
    {
        timer = 0;
        // Start async loading
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

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

            // Allow scene activation after minimum load time and progress is complete
            if (asyncLoad.progress >= 0.9f && timer >= minimumLoadTime)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        // Optional: Deactivate loading screen
    }
}
