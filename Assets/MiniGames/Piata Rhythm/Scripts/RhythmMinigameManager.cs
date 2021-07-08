using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmMinigameManager : MonoBehaviour
{
    public bool startMinigame;
    public BeatScroller beatScroller;
    public static RhythmMinigameManager singleton;

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;    
    }

    public void NoteHited()
    {
        print("acertou note");
    }

    public void NoteMissed()
    {
        print("perdeu note");
    }
}
