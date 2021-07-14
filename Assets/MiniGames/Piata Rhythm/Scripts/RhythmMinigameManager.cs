using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmMinigameManager : MonoBehaviour
{
    public bool startMinigame;
    public BeatScroller beatScroller;
    public AudioSource audioSource;

    public static RhythmMinigameManager singleton;

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;    
    }

    private void Update()
    {
        if(!startMinigame)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startMinigame = true;
                beatScroller.minigameStarted = true;

                audioSource.Play();
            }
    }

    public void NoteHited(string noteName)
    {
        RhythmEvents.NoteHit(noteName);
    }

    public void NoteMissed()
    {
    }
}
