using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public void NoteHited(string noteName)
    {
        RhythmMinigameManager.singleton.NoteHited(noteName);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("RhythmButton"))
        {
            other.gameObject.GetComponent<RhythmButtonController>().NoteEntered(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!gameObject.activeSelf)
            return;

        if (other.CompareTag("RhythmButton"))
        {
            other.gameObject.GetComponent<RhythmButtonController>().NoteExited();
            RhythmMinigameManager.singleton.NoteMissed();
        }
    }
}
