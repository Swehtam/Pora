using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmMinigameManager : MonoBehaviour
{
    public bool startMinigame;
    public BeatScroller beatScroller;
    public AudioSource audioSource;

    [Header("Planta��o")]
    public SpriteRenderer[] cropSpriteRenderers;
    public Sprite cropSeed;
    public Sprite cropWater;

    private int currentCrop = 0;

    public static RhythmMinigameManager singleton;

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;   
        foreach(SpriteRenderer sr in cropSpriteRenderers)
        {
            int randomValue = Random.Range(0, 2);
            if(randomValue == 0)
            {
                sr.sprite = cropSeed;
            }
            else
            {
                sr.sprite = cropWater;
            }
        }
    }

    private void Update()
    {
        if (!startMinigame)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startMinigame = true;
                beatScroller.minigameStarted = true;

                audioSource.Play();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startMinigame = false;
                beatScroller.minigameStarted = false;

                audioSource.Pause();
            }
        }
    }

    public void NoteHited(string noteName)
    {
        RhythmEvents.NoteHit(noteName);
        UpdateCropSpriteRenderer(noteName);
    }

    public void NoteMissed()
    {
    }

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
