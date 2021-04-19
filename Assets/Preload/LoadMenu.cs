using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    private LevelLoader levelLoader;
    void Start()
    {
        //Mudar para carregar a tela de menu, quando tiver uma
        levelLoader = InstancesManager.singleton.GetLevelLoaderInstance();
        levelLoader.LoadNextLevel("MainMenu");
    }
}
