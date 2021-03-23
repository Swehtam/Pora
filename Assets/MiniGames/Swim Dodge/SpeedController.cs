using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SpeedController : MonoBehaviour
{
    //Static para compartilhar a mesma velocidade com todos os objetos do Mini Game
    public static float speed = 0f;
    

    private DialogueRunner dialogueRunner;
    private void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        dialogueRunner.onDialogueComplete.AddListener(StartSwimming);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dialogueRunner.IsDialogueRunning)
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
}