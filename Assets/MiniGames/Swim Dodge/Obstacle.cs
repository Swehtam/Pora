using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Rigidbody2D rb;
    public int id;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(SpeedController.speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            var obstacle = collision.GetComponent<Obstacle>();
            //Se uma pedra e um tronco estiverem no mesmo ponto, então destroi a pedra (id: 0)
            if (id == 0 && obstacle.id == 1)
            {
                gameObject.SetActive(false);
            }
            //Se uma planta e um (tronco ou pedra) estiverem no mesmo ponto, então destroi a planta (id: 2 e 3)
            else if ((id == 2 || id == 3) && (obstacle.id == 0 || obstacle.id == 1))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
