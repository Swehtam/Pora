using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/*
 * Classe para salvar todos as informações do Player antes de sair do jogo
 * E para carregar todos essas informações quando iniciar o jogo
 */
public class PlayerSettings : MonoBehaviour
{
    public bool isTesting;
    public string playerStartPoint;
    public string playerStartMap;

    public bool gameFinished;
    public void Awake()
    {
        var target = Screen.currentResolution.refreshRate;
        Application.targetFrameRate = target;
        QualitySettings.vSyncCount = 0;
        //Fazer um sistema para saber qual foi o ultimo exitPoint em q o player foi carregado
        if (isTesting)
        {
            playerStartPoint = "Saida minigame";
            playerStartMap = "FazendaPiata";
        }
        else
        {
            playerStartPoint = "Cama";
            playerStartMap = "Casa";
        }
    }

    [YarnCommand("gameFinished")]
    public void GameFinished()
    {
        gameFinished = true;
        SaveLoadEvents.SaveGame();
    }

    public bool SavePlayerSettings()
    {
        return gameFinished;
    }

    public void LoadPlayerSettings(bool gameF)
    {
        gameFinished = gameF;
    }
}
