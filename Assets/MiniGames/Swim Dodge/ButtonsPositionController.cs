using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsPositionController : MonoBehaviour
{
    public GameObject pauseButton;
    public GameObject diveButton;
   
    public void ChangeButtonsSide()
    {
        RectTransform pauseRect = pauseButton.GetComponent<RectTransform>();
        //Mudar o Right e deixar o Top como está
        pauseRect.offsetMax = new Vector2(-686f, pauseRect.offsetMax.y);
        //Mudar o Left e deixar o Bottom como está
        pauseRect.offsetMin = new Vector2(93f, pauseRect.offsetMin.y);

        RectTransform diveRect = diveButton.GetComponent<RectTransform>();
        //Mudar o Right e deixar o Top como está
        diveRect.offsetMax = new Vector2(-623.5f, diveRect.offsetMax.y);
        //Mudar o Left e deixar o Bottom como está
        diveRect.offsetMin = new Vector2(30.5f, diveRect.offsetMin.y);
    }
}
