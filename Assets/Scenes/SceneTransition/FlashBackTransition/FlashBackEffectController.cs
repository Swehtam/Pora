using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashBackEffectController : MonoBehaviour
{
    public TMP_Text flashbackText;

    /// <summary>
    /// Metodo para atualizar o texto na tela quando for transicionar de um dia para o outro.
    /// </summary>s
    public void SetTextByID(int textID)
    {
        if (textID == 0)
        {
            flashbackText.text = "6 ANOS ATRÁS\n\nESCOLA - MANHÃ";
            return;
        }

        if(textID == 1)
        {
            flashbackText.text = "6 ANOS ATRÁS\n\nCUME DA VILA - TARDE";
            return;
        }

        if (textID == 2)
        {
            flashbackText.text = "ALGUMAS HORAS DEPOIS\n\nNOITE";
            return;
        }

        if (textID == 3)
        {
            flashbackText.text = "NO DIA SEGUINTE\n\nTARDE";
            return;
        }

        if (textID == 4)
        {
            flashbackText.text = "ALGUMAS HORAS DEPOIS\n\nCASA DE ACIR - TARDE";
            return;
        }
    }
}
