using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private GameObject eventSystem;

    public void SetEventSystem(GameObject eventS)
    {
        eventSystem = eventS;
    }

    //Usado para saber se o player encontrou um NPC
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NPC"))
            eventSystem.GetComponent<UIManager>().ShowTalkButton(other.gameObject);
    }

    //Usado para saber se o player saio de perto de um NPC
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NPC"))
            eventSystem.GetComponent<UIManager>().HideTalkButton();
    }
}
