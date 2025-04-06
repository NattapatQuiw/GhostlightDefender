using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameSceneManager : MonoBehaviour
{
    public UnityEvent<float> onProgressChanged;
    public UnityEvent onLoadComplete;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneGame(sceneName));
    }

    private IEnumerator LoadSceneGame(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            float currentProgress = Mathf.Clamp01(async.progress / 0.9f);
            onProgressChanged?.Invoke(currentProgress);
            
            if (currentProgress >= 0.9f)
            {
                onLoadComplete?.Invoke();
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
