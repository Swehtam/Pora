using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneEvents : MonoBehaviour
{
    public delegate void SceneLoadHandler();
    public static event SceneLoadHandler OnSceneLoad;

    public static void SceneLoading()
    {
        OnSceneLoad?.Invoke();
    }
}
