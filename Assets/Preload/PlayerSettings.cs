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
    public void Awake()
    {
        //Fazer um sistema para saber qual foi o ultimo exitPoint em q o player foi carregado
        playerStartPoint = "Ponto de inicio";
    }
}
