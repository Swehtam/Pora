using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryGameUI : MonoBehaviour
{
    //Finishing Panel
    [SerializeField] private GameObject finishingPanel;
    [SerializeField] private TMP_Text resultsText;

    //Animal Name Block
    [SerializeField] private Animator animalNameAnimator;
    [SerializeField] private TMP_Text animalNameText;

    //Animal Description Block
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TMP_Text animalNameDescriptionBlockText;
    [SerializeField] private TMP_Text animalDescriptionText;
    [SerializeField] private Image animalImageDescriptionBlock;

    //Timer
    [SerializeField] private TMP_Text timeText;

    //Show current phase of the minigame
    [SerializeField] private TMP_Text phaseText;

    //MemoryGameClassesInterface
    [SerializeField] private MemoryGameClassesInterface memoryGameClassesInterface;

    private PlayerController player;

    private void Start()
    {
        //Pega o player e seta que começou minigame
        player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();
        player.PlayingMinigame();

        MemoryGameEvents.OnTokenMatch += TokenMatch;
    }

    public void ShowDescriptionPanel()
    {
        descriptionPanel.SetActive(true);
        memoryGameClassesInterface.timer.StopTimer();
    }

    public void HideDescriptionPanel()
    {
        descriptionPanel.SetActive(false);
        memoryGameClassesInterface.timer.ResumeTimer();
    }

    public void ShowFinishingPanel(float results)
    {
        MemoryGameEvents.OnTokenMatch -= TokenMatch;
        finishingPanel.SetActive(true);
        results /= 10.0f;
        InstancesManager.singleton.GetInMemoryVariableStorage().SetValue("$classDone", true);
        InstancesManager.singleton.GetInMemoryVariableStorage().SetValue("$biologyDayResults", results);
        resultsText.text = string.Format("Resultado de hoje: {0:0.0}/10,0", results);
    }

    public void FinishContinueButton()
    {
        PlayerController player;
        player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();
        player.NotSiting();
        player.StopPlayingMinigame();
        player.loadPointName = "Saida Aula";
        //Aciona o evento de quest que completou o minigame
        InstancesManager.singleton.GetQuestEvents().BiologyClassCompleted();
        InstancesManager.singleton.GetDayManager().UpdateDayShift();
        InstancesManager.singleton.GetLevelLoaderInstance().LoadNextLevel("EscolaSalaAula", 0);
    }

    public void TokenMatch(string animalName, string animalDescription, Sprite animalImage)
    {
        animalNameText.text = animalName;
        animalNameDescriptionBlockText.text = animalName;
        animalDescriptionText.text = animalDescription;
        animalImageDescriptionBlock.sprite = animalImage;
        animalNameAnimator.SetTrigger("Start");
    }

    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void DisplayPhaseLevel(int phaseLevel)
    {
        phaseText.text = string.Format("{0}/3", phaseLevel);
    }
}
