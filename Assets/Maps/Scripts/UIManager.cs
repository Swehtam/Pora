using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject talkButton;
    [SerializeField] private GameObject moveButton;
    [SerializeField] private Button questButton;
    [SerializeField] private GameObject swimDodgePanel;
    [SerializeField] private GameObject questPanel;

    //Campo de texto para mostrar o dia e o turno
    [SerializeField] private TMP_Text dayDisplay;
    [SerializeField] private TMP_Text questTitle;
    [SerializeField] private TMP_Text questDescription;

    private YarnNPC npc;
    private DialogueRunner dialogueRunner;
    private PlayerController playerController;
    private DayManager dayManager;
    private Animator questButtonAnimator;
    private QuestEvents questEvents;
    private QuestsManager questsManager;

    void Start()
    {
        //Dialogo
        dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();

        //Dia
        dayManager = InstancesManager.singleton.GetDayManager();

        //Player
        var player = InstancesManager.singleton.GetPlayerInstance();
        playerController = player.GetComponent<PlayerController>();

        //Quest Button
        questButtonAnimator = questButton.GetComponent<Animator>();

        //Quest Event
        questEvents = InstancesManager.singleton.GetQuestEvents();
        questEvents.OnNewQuest += GotNewQuest;
        questEvents.OnNewQuestGoal += UpdateQuestGoal;

        //Load Scene Event
        LoadSceneEvents.OnSceneLoad += SceneLoading;

        //Quest Manager
        questsManager = InstancesManager.singleton.GetQuestsManager();
        if (questsManager.newQuest)
        {
            questButtonAnimator.SetBool("New_Quest", true);
        }
    }

    private void Update()
    {
        if(dayManager.GetDay() == 0)
        {
            dayDisplay.text = "O Grande Dia" + ", " + dayManager.GetStringDayShift();
        }
        else
        {
            dayDisplay.text = "Dia: " + dayManager.GetDay() + ", " + dayManager.GetStringDayShift();
        }

        //Se o dialogo estiver rodando, reseta o joystick para o player n sair andando sozinho quando acabar o dialogo
        if (dialogueRunner.IsDialogueRunning) 
        {
            moveButton.GetComponent<Joystick>().ResetJoystick();
            moveButton.SetActive(false);
            talkButton.SetActive(false);
            questButton.interactable = false;
        }
        else
        {
            moveButton.SetActive(true);
            questButton.interactable = true;
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

    public void OpenQuestPanel()
    {
        questButtonAnimator.SetBool("New_Quest", false);
        questsManager.newQuest = false;

        questPanel.SetActive(true);
        ActiveQuest q = questsManager.GetActiveQuest();
        if(q != null)
        {
            questTitle.text = q.title;
            questDescription.text = q.description;
        }
        else
        {
            questTitle.text = "Sem Missões";
            questDescription.text = "";
        }
    }

    public void CloseQuestPanel()
    {
        questPanel.SetActive(false);
    }

    public void GotNewQuest()
    {
        questButtonAnimator.SetBool("New_Quest", true);
        questsManager.newQuest = true;
    }

    public void UpdateQuestGoal(QuestGoal questGoal)
    {
        questButtonAnimator.SetBool("New_Quest", true);
        questsManager.newQuest = true;
    }

    public void DialogueButton()
    {
        //Acionar todos os eventos que dependem do player de falar com o NPC
        questEvents.NPCTalk(npc.characterName);
        //Começar o dialogo
        dialogueRunner.StartDialogue(npc.talkToNode);
    }

    public void SceneLoading()
    {
        questEvents.OnNewQuest -= GotNewQuest;
        questEvents.OnNewQuestGoal -= UpdateQuestGoal;
        LoadSceneEvents.OnSceneLoad -= SceneLoading;
    }

    //Mudar esse metodo de lugar
    public void LoadDreamLand()
    {
        playerController.loadPointName = "Cama Terra Sonhos";
        SceneManager.LoadScene("VilaLoboSonho");
    }
}
