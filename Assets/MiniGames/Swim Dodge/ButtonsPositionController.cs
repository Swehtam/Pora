using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsPositionController : MonoBehaviour
{
    public RectTransform pauseRect;
    public RectTransform diveRect;
    public RectTransform fpsRect;
   
    public void ChangeButtonsSide()
    {
        //Botão de pausa
        //Mudar o Right e deixar o Top como está
        pauseRect.offsetMax = new Vector2(-686f, pauseRect.offsetMax.y);
        //Mudar o Left e deixar o Bottom como está
        pauseRect.offsetMin = new Vector2(93f, pauseRect.offsetMin.y);

        //Botão de mergulho
        diveRect.anchoredPosition = new Vector2(diveRect.anchoredPosition.x * -1, diveRect.anchoredPosition.y);

        //FPS
        fpsRect.anchoredPosition = new Vector2(fpsRect.anchoredPosition.x * -1, fpsRect.anchoredPosition.y);
    }
}
