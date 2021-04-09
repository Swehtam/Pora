using UnityEngine;
using Yarn.Unity;

/// attached to the non-player characters, and stores the name of the Yarn
/// node that should be run when you talk to them.

public class YarnNPC : MonoBehaviour
{
    [Range(0f, 1f)]
    public float effect_amount;
    public string characterName = "";

    public string talkToNode = "";

    [Header("Optional")]
    public YarnProgram scriptToLoad;

    void Start()
    {
        if (scriptToLoad != null)
        {
            DialogueRunner dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
            dialogueRunner.Add(scriptToLoad);
        }

    }

    private void Update()
    {
        gameObject.GetComponent<Renderer>().material.SetFloat("_EffectAmount", effect_amount);
    }
}
