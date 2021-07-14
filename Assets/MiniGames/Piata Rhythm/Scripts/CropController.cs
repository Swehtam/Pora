using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{
    public SpriteRenderer[] cropSpriteRenderers;
    public Sprite cropSeed;
    public Sprite cropWater;

    private int currentCrop = 0;
    
    public void UpdateCropSpriteRenderer(string noteName)
    {
        if (noteName.Equals("seeds"))
        {
            cropSpriteRenderers[currentCrop].sprite = cropSeed;
        }
        else if (noteName.Equals("water"))
        {
            cropSpriteRenderers[currentCrop].sprite = cropWater;
        }

        //Atualiza a posi��o pra proxima planta��o
        //Se a plantacao estiver na ultima posi��o ent�o reseta
        if (currentCrop == cropSpriteRenderers.Length - 1)
        {
            currentCrop = 0;
        }
        //Se n�o acrescenta
        else
        {
            currentCrop++;
        }
    }
}