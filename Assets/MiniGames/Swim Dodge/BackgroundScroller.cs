using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startingPosition;
    private float width;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;

        rb = GetComponent<Rigidbody2D>();

        var collider = GetComponent<BoxCollider2D>();
        width = collider.size.x;
        collider.enabled = false;

        rb.velocity = new Vector2(SpeedController.speed, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x < -width)
        {
            //Coloquei esse -0.01f para poder sobrepor um pouco e não ficar aparecendo um espaço muito pequenos entre os mapas
            Vector2 resetPosition = new Vector2((width * 2f) - 0.01f, 0);
            transform.position = (Vector2) transform.position + resetPosition;
        }
        rb.velocity = new Vector2(SpeedController.speed, 0);
    }

    public void RestartGame()
    {

    }
}
