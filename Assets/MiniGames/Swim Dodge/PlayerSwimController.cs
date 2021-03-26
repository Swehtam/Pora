using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerSwimController : MonoBehaviour
{
    //A zone inicial do Player quando começar o mini game
    private int playerSwimZone = 2;
    //Número total de Zonas em que ele pode nadar
    public readonly int maxSwimZones = 3;
    //Altura das Zonas
    public readonly float swimZoneHeight = 2f;

    private bool isDiving = false;
    private Animator anim;
    private DialogueRunner dialogueRunner;

    private void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    private void Update()
    {
        if (DistanceController.isFirstHalfCompleted)
        {
            anim.SetBool("ChangeSide", true);
        }
    }

    public void MovePlayerUp()
    {
        if (!dialogueRunner.IsDialogueRunning)
        {
            //Move o player uma zona para cima e salva sua posição
            if (playerSwimZone < maxSwimZones)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + swimZoneHeight);
                playerSwimZone += 1;
            }
        }
    }

    public void MovePlayerDown()
    {
        if (!dialogueRunner.IsDialogueRunning)
        {
            //Move o player uma zona para baixo e salva sua posição
            if (playerSwimZone > 1)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - swimZoneHeight);
                playerSwimZone -= 1;
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
                Destroy(gameObject);
            }
            //Planta que empurra para baixo
            else if (obstacle.id == 2)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - swimZoneHeight);
                playerSwimZone -= 1;
            }
            //Planta que empurra para cima
            else if (obstacle.id == 3)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + swimZoneHeight);
                playerSwimZone += 1;
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
