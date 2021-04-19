using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    private void OnLevelWasLoaded()
    {
        StartNextLevel();
    }

    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }

    public void StartNextLevel()
    {
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
        transition.SetTrigger("End");

        yield return new WaitForSeconds(transitionTime);
    }
}
