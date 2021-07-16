using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmEvents : MonoBehaviour
{
    public delegate void NoteHitEvent(string noteName);
    public static event NoteHitEvent OnNoteHit;

    public delegate void NoteHittableEvent(string noteName);
    public static event NoteHittableEvent OnNoteHittable;

    public static void NoteHit(string noteName)
    {
        OnNoteHit?.Invoke(noteName);
    }

    public static void NoteHittable(string noteName)
    {
        OnNoteHittable?.Invoke(noteName);
    }
}