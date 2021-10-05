using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadEvents : MonoBehaviour
{
    public delegate void GameSaveHandler();
    public static event GameSaveHandler OnGameSave;
    public delegate void GameLoadHandler();
    public static event GameLoadHandler OnGameLoad;

    public static void SaveGame()
    {
        OnGameSave?.Invoke();
    }

    public static void LoadGame()
    {
        OnGameLoad?.Invoke();
    }
}
