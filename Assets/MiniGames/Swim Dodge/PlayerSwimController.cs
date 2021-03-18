using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwimController : MonoBehaviour
{
    //A zone inicial do Player quando começar o mini game
    private int playerSwimZone = 2;
    //Número total de Zonas em que ele pode nadar
    public readonly int maxSwimZones = 3;
    //Altura das Zonas
    public readonly float swimZoneHeight = 2f;
    public event Action OnReset;
    private int score = 0;

    public void MovePlayerUp()
    {
        //Move o player uma zona para cima e salva sua posição
        if (playerSwimZone < maxSwimZones)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + swimZoneHeight);
            playerSwimZone += 1;
        }
    }

    public void MovePlayerDown()
    {
        //Move o player uma zona para baixo e salva sua posição
        if (playerSwimZone > 1)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - swimZoneHeight);
            playerSwimZone -= 1;
        }
    }

    private void Reset()
    {
        score = 0;
        OnReset?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("score"))
        {
            score++;
            ScoreCollector.Instance.AddScore(score);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
