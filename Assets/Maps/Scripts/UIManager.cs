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

        moveButton.SetActive(!dialogueRunner.IsDialogueRunning); //Mostrar o botão de andar se n tiver dialogo, caso contrario não mostrar
        if (dialogueRunner.IsDialogueRunning) talkButton.SetActive(false); //Se tiver dialogo não mostrar o botão de interagir
    }

    public void ShowTalkButton(YarnNPC npcYarnScript)
    {
        talkButton.SetActive(true);
        npc = npcYarnScript;
    }

    public void HideTalkButton()
    {
        talkButton.SetActive(false);
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
