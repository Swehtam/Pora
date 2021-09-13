using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SleepEffectController : MonoBehaviour
{
    public TMP_Text transitionText;
    private DayManager dayManager;

    private void Start()
    {
        dayManager = InstancesManager.singleton.GetDayManager();
    }

    /// <summary>
    /// Metodo para atualizar o texto na tela quando for transicionar de um dia para o outro.
    /// </summary>s
    public void SetTextByID(int textID)
    {
        if (textID == 0)
        {
            transitionText.text = "O GRANDE DIA";
            return;
        }

        if(textID == 1)
        {
            transitionText.text = "DE VOLTA AO PRESENTE - CASA";
            return;
        }

        transitionText.text = "Dia " + dayManager.GetDay();
    }
}
