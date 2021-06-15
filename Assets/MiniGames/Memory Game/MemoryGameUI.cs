using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MemoryGameUI : MonoBehaviour
{
    //Finishing Panel
    [SerializeField] private GameObject finishingPanel;
    [SerializeField] private TMP_Text resultsText;

    //Animal Name Block
    [SerializeField] private Animator animalNameAnimator;
    [SerializeField] private TMP_Text animalNameText;

    //Timer
    [SerializeField] private TMP_Text timeText;

    //Show current phase of the minigame
    [SerializeField] private TMP_Text phaseText;

    private void Start()
    {
        MemoryGameEvents.OnTokenMatch += TokenMatch;
    }

    public void ShowFinishingPanel(float results)
    {
        finishingPanel.SetActive(true);
        resultsText.text = string.Format("Resultado de hoje: {0:0.0}/10,0", results/10.0f);
    }

    public void FinishContinueButton()
    {
        PlayerController player;
        player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();
        player.loadPointName = "Saida Aula";
        InstancesManager.singleton.GetLevelLoaderInstance().LoadNextLevel("EscolaSalaAula", 0);
    }

    public void TokenMatch(string animalName)
    {
        animalNameText.text = animalName;
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
