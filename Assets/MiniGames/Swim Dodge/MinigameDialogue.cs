using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MinigameDialogue : MonoBehaviour
{
    [Header("Optional")]
    public YarnProgram scriptToLoad;

    [SerializeField] private MinigameClassesInterface minigameClassesInterface;
    // Start is called before the first frame update
    void Start()
    {
        //dialogueRunner = InstancesManager.singleton.getDialogueRunnerInstance();
        if(scriptToLoad != null)
        {
            minigameClassesInterface.dialogueRunner.Add(scriptToLoad);
        }

        if(!SwimDodgeTutorialPanel.IsFirstTutorial)
            StartFirstDialogue();
    }

    public void StartFirstDialogue()
    {
        minigameClassesInterface.dialogueRunner.StartDialogue("SwimDodgeMinigame.FirstSide");
    }

    public void StartSecondDialogue()
    {
        minigameClassesInterface.dialogueRunner.StartDialogue("SwimDodgeMinigame.SecondSide");
    }
}
