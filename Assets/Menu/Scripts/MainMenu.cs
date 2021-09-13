using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        PlayerSettings playerSettings = InstancesManager.singleton.GetPlayerSettingsInstance();
        int day = InstancesManager.singleton.GetDayManager().GetDay();
        //Mudar para carregar a tela de menu, quando tiver uma
        if (day == 0)
        {
            InstancesManager.singleton.GetLevelLoaderInstance().MainMenuTransition(playerSettings.playerStartMap, day.ToString());
            return;
        }
        InstancesManager.singleton.GetLevelLoaderInstance().LoadNextLevel(playerSettings.playerStartMap, 2);
    }

    public void QuitGame()
    {
        //Fechar o jogo
        Debug.Log("Fechou!");
        Application.Quit();
    }
}
