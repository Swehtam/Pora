using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMinigame : MonoBehaviour
{
    public GameObject pauseMenuUI;

    //Eventos para quando pausar e despausar
    public UnityEvent onPause;
    public UnityEvent onResume;
    public UnityEvent onQuit;
    public UnityEvent onRestart;
    public UnityEvent onShowTutorial;

    public void PauseGame()
    {
        onPause?.Invoke();

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        onResume?.Invoke();

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        onQuit?.Invoke();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        onRestart?.Invoke();
    }

    public void ShowTutorial()
    {
        onShowTutorial?.Invoke();
    }
}
