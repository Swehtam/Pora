using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        PlayerSettings playerSettings = InstancesManager.singleton.GetPlayerSettingsInstance();
        //Mudar para carregar a tela de menu, quando tiver uma
        InstancesManager.singleton.GetLevelLoaderInstance().LoadNextLevel(playerSettings.playerStartMap, 2);
    }

    public void QuitGame()
    {
        //Fechar o jogo
        Debug.Log("Fechou!");
        Application.Quit();
    }
}
