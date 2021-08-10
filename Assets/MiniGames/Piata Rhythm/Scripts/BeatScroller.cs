using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo;

    public bool minigameStarted;

    public bool minigameLost;

    private float t;
    private float beatAux; //Varivel auxiliar para interpolar a velocidade do scroller

    void Start()
    {
        beatTempo /= 60f;
        beatAux = beatTempo;
    }

    void Update()
    {
        if(minigameStarted)
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);

        //Caso o player tenha perdido o minigame faça com que o scroller pare aos poucos
        if (minigameLost && beatTempo != 0)
        {
            //1 segundo para parar a velocidade
            t += Time.deltaTime / 1f;
            beatTempo = Mathf.Lerp(beatAux, 0f, t);

            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
    }
}
