using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SpeedController : MonoBehaviour
{
    //Static para compartilhar a mesma velocidade com todos os objetos do Mini Game
    public static float speed = 0f;

    private bool playerLost = false; //Variavel para controlar a velocidade caso o player perca o minigame
    private float t;
    private float speedAux; //Varivel auxiliar para interpolar a velocidade do jogo
    private DialogueRunner dialogueRunner;
    private void Start()
    {
        speed = 0f;
        dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
        dialogueRunner.onDialogueComplete.AddListener(StartSwimming);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dialogueRunner.IsDialogueRunning && !SwimDodgeTutorialPanel.IsTutorialRunning && !playerLost)
        {
            if (!DistanceController.isFirstHalfCompleted)
            {
                //ajusta a velocidade do player para a direita
                if (speed >= -5f)
                {
                    speed += Time.deltaTime * -0.04f;
                }
            }
            else
            {
                //Ajusta a velocidade do player para a esquerda
                if (speed <= 5f)
                {
                    speed += Time.deltaTime * 0.04f;
                }
            }
        }

        if (playerLost && speed != 0f)
        {
            //1 segundo para parar a velocidade
            t += Time.deltaTime / 1f;
            speed = Mathf.Lerp(speedAux, 0f, t);
        }
    }

    private void StartSwimming()
    {
        if (DistanceController.isFirstHalfCompleted)
        {
            speed = 4f;
        }
        else
        {
            speed = -3f;
        }
    }

    public void ResetGame()
    {
        speed = 0f;   
    }

    public void PlayerLost()
    {
        speedAux = speed;
        playerLost = true;
    }
}