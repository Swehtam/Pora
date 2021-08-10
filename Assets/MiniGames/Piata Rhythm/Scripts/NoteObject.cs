using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public void NoteHited()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("RhythmButton"))
        {
            other.gameObject.GetComponent<RhythmButtonController>().NoteEntered(this);
            RhythmEvents.NoteHittable(other.gameObject.GetComponent<RhythmButtonController>().GetNoteName());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!gameObject.activeSelf)
            return;

        if (other.CompareTag("RhythmButton"))
        {
            other.gameObject.GetComponent<RhythmButtonController>().NoteExited(this);
            RhythmMinigameManager.singleton.NoteMissed();
        }
    }
}
