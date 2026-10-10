using System;
using System.Collections;
using ConquerTheStars.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    private IEnumerator Start()
    {
        var gameManager = GameManager.Instance;

        if (gameManager == null || string.IsNullOrEmpty(gameManager.PendingSceneName))
        {
            Debug.LogError("Load need to open equal Game Manager");
            yield break;
        }

        var targetScene = gameManager.PendingSceneName;

        yield return new WaitForSecondsRealtime(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);

        yield return operation;
    }
}
