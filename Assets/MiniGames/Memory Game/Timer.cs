using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private MemoryGameClassesInterface memoryGameClassesInterface;

    private float timeRemaining;
    private bool timerIsRunning = false;

    public void StartTimer(int difficultyTime)
    {
        timeRemaining = difficultyTime;
        timerIsRunning = true;
    }

    void FixedUpdate()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.fixedDeltaTime;
                memoryGameClassesInterface.memoryGameUI.DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                memoryGameClassesInterface.memoryGameController.FinishMinigame();
            }
        }
    }

    public void StopTimer()
    {
        timerIsRunning = false;
    }

    public void ResumeTimer()
    {
        timerIsRunning = true;
    }
}