using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerSwimController : MonoBehaviour
{
    //Número total de Zonas em que ele pode nadar
    public readonly int maxSwimZones = 3;
    //Altura das Zonas
    public readonly float swimZoneHeight = 2f;
    public GameObject loseMenu;
    [SerializeField] private MinigameClassesInterface minigameClassesInterface;

    //A zone inicial do Player quando começar o mini game
    private int playerSwimZone = 2;
    private bool isDiving = false;
    private bool playerLost = false;
    private bool movingPlayer = false;
    private Vector3 positionAux;
    private Vector3 targetAux;
    private float t;
    private Animator anim;
    private DialogueRunner dialogueRunner;
    
    private void Start()
    {
        dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (DistanceController.isFirstHalfCompleted)
        {
            anim.SetBool("ChangeSide", true);
        }

        if (movingPlayer)
        {
            //mover o player na mesma velocidade que se encontra a velocidade do jogo
            t += Time.deltaTime / 0.1f;
            transform.position = Vector3.Lerp(positionAux, targetAux, t);
            //Se chegou aonde queria entao para
            if (transform.position == targetAux)
            {
                t = 0f;
                movingPlayer = false;
            }
        }

    }

    public void MovePlayerUp()
    {
        if (!dialogueRunner.IsDialogueRunning && !SwimDodgeTutorialPanel.IsTutorialRunning && !playerLost && !movingPlayer)
        {
            //Move o player uma zona para cima e salva sua posição
            if (playerSwimZone < maxSwimZones)
            {
                positionAux = transform.position; //Salva a posição que o player se encontra
                targetAux = new Vector3(positionAux.x, positionAux.y + swimZoneHeight, positionAux.z); //Marca para onde ele deve ir
                movingPlayer = true; //Sinaliza que o player esta movendo
                playerSwimZone += 1; //Aumenta a posição que o player se encontra
            }
        }
    }

    public void MovePlayerDown()
    {
        if (!dialogueRunner.IsDialogueRunning && !SwimDodgeTutorialPanel.IsTutorialRunning && !playerLost && !movingPlayer)
        {
            //Move o player uma zona para baixo e salva sua posição
            if (playerSwimZone > 1)
            {
                positionAux = transform.position; //Salva a posição que o player se encontra
                targetAux = new Vector3(positionAux.x, positionAux.y - swimZoneHeight, positionAux.z); //Marca para onde ele deve ir
                movingPlayer = true; //Sinaliza que o player esta movendo
                playerSwimZone -= 1; //Diminui a posição que o player se encontra
            }
        }
    }

    //O player morre se bater em uma pedra, mergulhando ou não
    //O player morre se bater em um tronco de árvore sem mergulhar
    //O player é empurrado para baixo se bater em uma planta que jogue-o para baixo
    //O player é empurrado para cima se bater em uma planta que jogue-o para cima
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            var obstacle = collision.GetComponent<Obstacle>();
            if(obstacle.id == 0 || (obstacle.id == 1 && !isDiving))
            {
                Destroy(gameObject); //Destroi o player
                playerLost = true; //Sinaliza q o player perder
                minigameClassesInterface.speedController.PlayerLost(); //Avisa ao Speed Controller que o player perdeu
                minigameClassesInterface.obstaclesManager.PlayerLost(); //Avisa ao Obstacle Manager que o player perdeu
                loseMenu.SetActive(true); //Ativa a interface de derrota do player
            }
            //Planta que empurra para baixo
            else if (obstacle.id == 2)
            {
                //Se o player se mover em direção a uma planta
                if (movingPlayer)
                {
                    /*positionAux = targetAux;
                    targetAux = new Vector3(positionAux.x, positionAux.y - swimZoneHeight, positionAux.z); //Marca para onde ele deve ir
                    movingPlayer = true; //Sinaliza que o player esta movendo
                    playerSwimZone -= 1; //Diminui a posição que o player se encontra*/
                    t = 0f;
                    positionAux = transform.position; //Salva a posição que o player se encontra
                    targetAux = new Vector3(targetAux.x, targetAux.y - swimZoneHeight, targetAux.z); //Marca para onde ele deve ir
                    movingPlayer = true; //Sinaliza que o player esta movendo
                    playerSwimZone -= 1; //Diminui a posição que o player se encontra
                }
                else
                {
                    positionAux = transform.position; //Salva a posição que o player se encontra
                    targetAux = new Vector3(positionAux.x, positionAux.y - swimZoneHeight, positionAux.z); //Marca para onde ele deve ir
                    movingPlayer = true; //Sinaliza que o player esta movendo
                    playerSwimZone -= 1; //Diminui a posição que o player se encontra
                }
            }
            //Planta que empurra para cima
            else if (obstacle.id == 3)
            {
                if (movingPlayer)
                {
                    t = 0f;
                    positionAux = transform.position; //Salva a posição que o player se encontra
                    targetAux = new Vector3(targetAux.x, targetAux.y + swimZoneHeight, targetAux.z); //Marca para onde ele deve ir
                    movingPlayer = true; //Sinaliza que o player esta movendo
                    playerSwimZone += 1; //Aumena a posição que o player se encontra
                }
                else
                {
                    positionAux = transform.position; //Salva a posição que o player se encontra
                    targetAux = new Vector3(positionAux.x, positionAux.y + swimZoneHeight, positionAux.z); //Marca para onde ele deve ir
                    movingPlayer = true; //Sinaliza que o player esta movendo
                    playerSwimZone += 1; //Aumena a posição que o player se encontra
                }
            }
        }
    }

    //Faz com q Porã mergulhe
    public void PlayerDive()
    {
        anim.SetBool("IsDiving", true);
        isDiving = true;
    }

    //Faz com q Porã suba
    public void PlayerEmerge()
    {
        anim.SetBool("IsDiving", false);
        isDiving = false;
    }
}
