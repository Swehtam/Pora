using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SleepEffectController : TransitionEffectController
{
    public TMP_Text transitionText;
    private DayManager dayManager;

    private void Start()
    {
        dayManager = InstancesManager.singleton.GetDayManager();
    }

    /// <summary>
    /// Metodo que deve ser chamado para transicionar para um scena do passado, ou colocar um texto mais especifico
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        transitionText.text = text;
    }

    /// <summary>
    /// Metodo para atualizar o texto na tela quando for transicionar de um dia para o outro.
    /// </summary>
    public void AutoSetText()
    {
        if (dayManager.GetDay() == 0)
        {
            transitionText.text = "O GRANDE DIA";
            return;
        }
        transitionText.text = "Dia " + dayManager.GetDay();
    }
}
