using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject talkButton;
    [SerializeField] private GameObject moveButton;
    [SerializeField] private GameObject swimDodgePanel;

    private YarnNPC npc;
    private DialogueRunner dialogueRunner;
    private PlayerController playerController;
    

    //Campo de texto para mostrar o dia e o turno
    [SerializeField] private TMP_Text dayDisplay;

    void Start()
    {
        dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
        var player = InstancesManager.singleton.GetPlayerInstance();
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        dayDisplay.text = "Dia: " + DayManager.GetDay() + ", " + DayManager.GetStringDayShift();

        //Se o dialogo estiver rodando, reseta o joystick para o player n sair andando sozinho quando acabar o dialogo
        if (dialogueRunner.IsDialogueRunning) 
        {
            moveButton.GetComponent<Joystick>().ResetJoystick();
            moveButton.SetActive(false);
            talkButton.SetActive(false);
        }
        else
        {
            moveButton.SetActive(true);
        }
    }

    public void ShowTalkButton(YarnNPC npcYarnScript)
    {
        talkButton.SetActive(true);
        npc = npcYarnScript;
    }

    [YarnCommand("showSwimDodgePanel")]
    public void ShowSwimDodgePanel()
    {
        swimDodgePanel.SetActive(true);
    }

    public void HideTalkButton()
    {
        talkButton.SetActive(false);
    }

    public void HideSwimDodgePanel()
    {
        swimDodgePanel.SetActive(false);
    }

    public void DialogueButton()
    {
        dialogueRunner.StartDialogue(npc.talkToNode);
    }

    //Mudar esse metodo de lugar
    public void LoadDreamLand()
    {
        playerController.loadPointName = "Cama Terra Sonhos";
        SceneManager.LoadScene("VilaLoboSonho");
    }
}
