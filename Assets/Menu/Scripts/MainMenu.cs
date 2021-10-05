using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Button extraButton;
    public TMP_Text extraText;
    public Button continueButton;
    public TMP_Text continueText;

    public Color disableColor;

    private void Start()
    {
        //Se o jogo ja tiver sido finalizado antes, então deixa clicar em Extra
        if (!InstancesManager.singleton.GetPlayerSettingsInstance().gameFinished)
        {
            extraButton.interactable = false;
            extraText.color = disableColor;
        }

        if (!InstancesManager.singleton.GetSaveSystem().fileLoaded)
        {
            continueButton.interactable = false;
            continueText.color = disableColor;
        }
    }

    public void StartGame()
    {
        PlayerSettings playerSettings = InstancesManager.singleton.GetPlayerSettingsInstance();
        int day = InstancesManager.singleton.GetDayManager().GetDay();
        //Mudar para carregar a tela de menu, quando tiver uma
        if (day == 0)
        {
            InstancesManager.singleton.GetLevelLoaderInstance().MainMenuTransition(playerSettings.playerStartMap, day);
            return;
        }
        InstancesManager.singleton.GetLevelLoaderInstance().LoadNextLevel(playerSettings.playerStartMap, 2);
    }

    public void ContinueGame()
    {
        SaveLoadEvents.LoadGame();
        StartGame();
    }

    public void ExtraPanel()
    {

    }

    public void QuitGame()
    {
        //Fechar o jogo
        Debug.Log("Fechou!");
        Application.Quit();
    }
}
