using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject talkButton;

    private YarnNPC npc;
    private DialogueRunner dialogueRunner;
    private PlayerController playerController;
    private PlayerInteractionController playerInteraction;

    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
        var player = InstancesManager.singleton.GetPlayerInstance();
        playerController = player.GetComponent<PlayerController>();
        playerInteraction = player.GetComponentInChildren<PlayerInteractionController>();

        playerInteraction.SetEventSystem(gameObject);
    }

    public void ShowTalkButton(GameObject npcGameObject)
    {
        talkButton.SetActive(true);
        npc = npcGameObject.GetComponent<YarnNPC>();
    }

    public void HideTalkButton()
    {
        talkButton.SetActive(false);
    }

    public void DialogueButton()
    {
        dialogueRunner.StartDialogue(npc.talkToNode);
    }

    public void LoadDreamLand()
    {
        playerController.loadPointName = "Cama Terra Sonhos";
        SceneManager.LoadScene("VilaLoboSonho");
    }
}
