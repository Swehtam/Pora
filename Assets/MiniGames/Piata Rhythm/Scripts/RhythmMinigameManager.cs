using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RhythmMinigameManager : MonoBehaviour
{
    public bool startMinigame;
    public BeatScroller beatScroller;
    public AudioSource audioSource;

    [Header("Plantação")]
    public SpriteRenderer[] cropSpriteRenderers;
    public Sprite cropSeed;
    public Sprite cropWater;

    [Header("Pontuação")]
    public int totalNotes;
    public SpriteRenderer[] basketSpriteRenderers;
    public Sprite fruitBasket;

    private int currentCrop = 0;
    private int currentHitNotes = 0;

    public static RhythmMinigameManager singleton;

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        RhythmEvents.OnNoteHit += singleton.NoteHited;
        foreach (SpriteRenderer sr in cropSpriteRenderers)
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

    void OnDisable()
    {
        RhythmEvents.OnNoteHit -= singleton.NoteHited;
    }

    private void Update()
    {
        if (startMinigame)
        {
            if (!audioSource.isPlaying)
            {
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startMinigame = false;
                beatScroller.minigameStarted = false;

                audioSource.Pause();
            }
        }
    }

    public void StartMinigame()
    {
        startMinigame = true;
        beatScroller.minigameStarted = true;

        audioSource.Play();
    }

    public void NoteHited(string noteName)
    {
        UpdateCropSpriteRenderer(noteName);
        UpdateScore();
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

        //Atualiza a posição pra proxima plantação
        //Se a plantacao estiver na ultima posição então reseta
        if (currentCrop == cropSpriteRenderers.Length - 1)
        {
            currentCrop = 0;
        }
        //Se não acrescenta
        else
        {
            currentCrop++;
        }
    }

    private void UpdateScore()
    {
        currentHitNotes++;
        float percentageHit = currentHitNotes * 100f / totalNotes ;

        //Atualiza a sprite da cesta de acordo com a porcentagem de acerto
        //Acerto acima de 95%
        if (percentageHit >= 95f)
        {
            basketSpriteRenderers[4].sprite = fruitBasket;
            return;
        }

        if(percentageHit >= 75f && percentageHit < 95f)
        {
            basketSpriteRenderers[3].sprite = fruitBasket;
            return;
        }

        if (percentageHit >= 55f && percentageHit < 75f)
        {
            basketSpriteRenderers[2].sprite = fruitBasket;
            return;
        }

        if (percentageHit >= 35f && percentageHit < 55f)
        {
            basketSpriteRenderers[1].sprite = fruitBasket;
            return;
        }

        if (percentageHit >= 15f && percentageHit < 35f)
        {
            basketSpriteRenderers[0].sprite = fruitBasket;
            return;
        }
    } 
}
