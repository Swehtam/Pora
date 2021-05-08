using UnityEngine;
using Yarn.Unity;

/// attached to the non-player characters, and stores the name of the Yarn
/// node that should be run when you talk to them.

public class YarnNPC : MonoBehaviour
{
    public string characterName = "";

    public string talkToNode = "";

    [Header("Optional")]
    public YarnProgram scriptToLoad;

    private UIManager uIManager;

    void Start()
    {
        if (scriptToLoad != null)
        {
            DialogueRunner dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
            dialogueRunner.Add(scriptToLoad);
        }

        uIManager = InstancesManager.singleton.GetUIManager();
    }

    //Quando o NPC encontrar o Player mostrar o botão para falar
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            uIManager.ShowTalkButton(this);
    }

    //Usado para saber se o player saio de perto do NPC
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            uIManager.HideTalkButton();
    }
}
