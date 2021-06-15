using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MinigameDialogue : MonoBehaviour
{
    [Header("Optional")]
    public YarnProgram scriptToLoad;

    private DialogueRunner dialogueRunner;
    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();

        if (scriptToLoad != null)
        {
            dialogueRunner.Add(scriptToLoad);
        }

        if(!SwimDodgeTutorialPanel.IsFirstTutorial)
            StartFirstDialogue();
    }

    public void StartFirstDialogue()
    {
        MinigamesManager.UpdateSwimDodgeMaxDistance();
        dialogueRunner.StartDialogue("SwimDodgeMinigame.FirstSide");
    }

    public void StartSecondDialogue()
    {
        dialogueRunner.StartDialogue("SwimDodgeMinigame.SecondSide");
    }

    public void StartFinishingDialogue()
    {
        dialogueRunner.StartDialogue("SwimDodgeMinigame.FinishingMinigame");
    }
}
