using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Classe para salvar todos as informações do Player antes de sair do jogo
 * E para carregar todos essas informações quando iniciar o jogo
 */
public class PlayerSettings : MonoBehaviour
{
    public string playerStartPoint;
    public string playerStartMap;
    public void Awake()
    {
        var target = Screen.currentResolution.refreshRate;
        Application.targetFrameRate = target;
        QualitySettings.vSyncCount = 0;
        //Fazer um sistema para saber qual foi o ultimo exitPoint em q o player foi carregado
        playerStartPoint = "Cama";
        playerStartMap = "Casa";
        //playerStartPoint = "Entrada Sala de Aula";
        //playerStartMap = "EscolaSalaAula";
    }
}
