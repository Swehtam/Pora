using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    private Rigidbody2D rb;
    private float width;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        var collider = GetComponent<BoxCollider2D>();
        width = collider.size.x;
        collider.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(SpeedController.speed, 0);
        if (!DistanceController.isFirstHalfCompleted)
        {
            //São 3 backgrounds, 1 no meio e mais 1 em cada lado do background do meio, somando 3
            //O do meio é width/2 unidades para a esquerda e width/2 unidades para a direita
            //Então para que o background volte para o final da fila, ele tem q estar a distância de 2,5 backgrounds da camera
            //
            if (transform.position.x < -2.5f * width)
            {
                //Coloquei esse -0.01f para poder sobrepor um pouco e não ficar aparecendo um espaço muito pequenos entre os mapas
                Vector2 resetPosition = new Vector2((width * 3f) - 0.2f, 0);
                transform.position = (Vector2)transform.position + resetPosition;
            }
        }
        else
        {
            //Nessa caso a mulitplicação é de 1.5 pois o ponto pivo dos backgrounds é no topo esqurda, então o proprio tamanho do background é tirado dessa expressão
            //Subtrai de 13 pois é o quanto a camera andou para a esquerda
            if (transform.position.x > (1.5f * width) - 13f)
            {
                //Coloquei esse +0.01f para poder sobrepor um pouco e não ficar aparecendo um espaço muito pequenos entre os mapas
                Vector2 resetPosition = new Vector2((width * -3f) + 0.2f, 0);
                transform.position = (Vector2)transform.position + resetPosition;
            }
        }
    }
}