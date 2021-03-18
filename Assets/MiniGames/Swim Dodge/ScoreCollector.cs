using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCollector : MonoBehaviour
{
    public static ScoreCollector Instance;
    [SerializeField] private TMP_Text display;
    private int highScore = 0;
    void Awake()
    {
        Instance = this;
    }

    public void AddScore(int score)
    {
        display.text = score.ToString();
        if (score > highScore)
        {
            highScore = score;
        }
    }
}
