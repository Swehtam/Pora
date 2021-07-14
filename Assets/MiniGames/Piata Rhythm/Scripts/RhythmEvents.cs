using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmEvents : MonoBehaviour
{
    public delegate void NoteHitEvent(string noteName);
    public static event NoteHitEvent OnNoteHit;

    public static void NoteHit(string noteName)
    {
        OnNoteHit?.Invoke(noteName);
    }
}