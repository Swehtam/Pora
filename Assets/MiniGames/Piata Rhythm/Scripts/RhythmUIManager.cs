using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmUIManager : MonoBehaviour
{
    public Animator piataAnimator;

    void Start()
    {
        RhythmEvents.OnNoteHittable += NoteHittable;
    }

    void OnDisable()
    {
        RhythmEvents.OnNoteHittable -= NoteHittable;
    }

    private void NoteHittable(string noteName)
    {
        if (noteName.Equals("seeds"))
        {
            piataAnimator.SetTrigger("Seeds");
            return;
        }

        if (noteName.Equals("water"))
        {
            piataAnimator.SetTrigger("Water");
            return;
        }
    }
}
