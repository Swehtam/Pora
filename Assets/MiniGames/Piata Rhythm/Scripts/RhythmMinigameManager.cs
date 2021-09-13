using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmMinigameManager : MonoBehaviour
{
    public bool startMinigame;
    public BeatScroller beatScroller;
    public AudioSource audioSource;
    public RhythmUIManager rhythmUIManager;
    public SwimDodgeTutorialPanel tutorialPanel;

    [Header("Plantação")]
    public SpriteRenderer[] cropSpriteRenderers;
    public Sprite cropSeed;
    public Sprite cropWater;

    [Header("Pontuação")]
    public int totalNotes;
    public SpriteRenderer[] basketSpriteRenderers;
    public Sprite fruitBasket;

    [Header("Notas Perdidas")]
    public SpriteRenderer[] boardSpriteRenderers;
    public Sprite redCrossBoard;

    private bool minigameStarted = false;
    private bool isPaused = false;
    private int currentCrop = 0;
    private int currentHitNotes = 0;
    private int currentMissedNotesInRow = 0;
    private float percentageHit = 0.0f;
    private int currentRank = 0;

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

    void Update()
    {
        if (!isPaused)
        {
            if (minigameStarted && !audioSource.isPlaying)
            {
                FinishMinigame();
            }
        }
    }

    void OnDisable()
    {
        RhythmEvents.OnNoteHit -= singleton.NoteHited;
    }

    public void StartMinigame()
    {
        beatScroller.minigameStarted = true;
        minigameStarted = true;

        audioSource.Play();
    }

    public void PauseMinigame()
    {
        beatScroller.minigameStarted = false;
        isPaused = true;

        audioSource.Pause();
    }

    public void ResumeMinigame()
    {
        //Primeiro tenho que despausar a musica, se não no Update vai achar que acabou o minigame
        audioSource.UnPause();

        beatScroller.minigameStarted = true;
        isPaused = false;
    }

    public void LostMinigame()
    {
        beatScroller.minigameStarted = false;
        beatScroller.minigameLost = true;

        audioSource.Pause();
        rhythmUIManager.ShowLostPanel();
    }

    public void FinishMinigame()
    {
        beatScroller.minigameStarted = false;

        print(currentRank);
        rhythmUIManager.ShowFinishPanel(percentageHit, currentRank);
    }

    public void NoteHited(string noteName)
    {
        ResetBoards();
        UpdateCropSpriteRenderer(noteName);
        UpdateScore();
    }

    public void NoteMissed()
    {
        //Atualiza a sprite
        if (currentMissedNotesInRow < 3)
        {
            boardSpriteRenderers[currentMissedNotesInRow].sprite = redCrossBoard;
        }

        //Aumenta a quantidade de erros seguidos
        currentMissedNotesInRow++;

        //Se for igual a 3 ou maior então diz q o player perdeu
        if (currentMissedNotesInRow >= 3)
        {
            LostMinigame();
        }
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

    private void ResetBoards()
    {
        //Diz que o jogador não tem mais erros
        currentMissedNotesInRow = 0;

        foreach (SpriteRenderer sr in boardSpriteRenderers)
        {
            sr.sprite = null;
        }
    }

    private void UpdateScore()
    {
        currentHitNotes++;
        percentageHit = currentHitNotes * 100f / totalNotes ;

        //Atualiza a sprite da cesta de acordo com a porcentagem de acerto
        //Acerto acima de 95%
        if (percentageHit >= 95f)
        {
            basketSpriteRenderers[4].sprite = fruitBasket;
            currentRank = 5;
            return;
        }

        if(percentageHit >= 75f && percentageHit < 95f)
        {
            basketSpriteRenderers[3].sprite = fruitBasket;
            currentRank = 4;
            return;
        }

        if (percentageHit >= 55f && percentageHit < 75f)
        {
            basketSpriteRenderers[2].sprite = fruitBasket;
            currentRank = 3;
            return;
        }

        if (percentageHit >= 35f && percentageHit < 55f)
        {
            basketSpriteRenderers[1].sprite = fruitBasket;
            currentRank = 2;
            return;
        }

        if (percentageHit >= 15f && percentageHit < 35f)
        {
            basketSpriteRenderers[0].sprite = fruitBasket;
            currentRank = 1;
            return;
        }
    } 
}
