using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RhythmUIManager : MonoBehaviour
{
    public Animator piataAnimator;
    public GameObject pauseButton;
    public GameObject lostPanel;

    public Animator timerAnimator;

    [Header("Finish Panel")]
    public GameObject finishPanel;
    public TMP_Text hitText;
    public TMP_Text rankText;

    private int rankValue;
    private PlayerController player;

    void Start()
    {
        //Tirar o comentario abaixo quando juntar o minigame da fazenda com o resto do jogo
        player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();
        if(player != null)
            player.PlayingMinigame();

        RhythmEvents.OnNoteHittable += NoteHittable;
    }

    void Update()
    {
        if (timerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Start_Timer") 
            && timerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            pauseButton.SetActive(true);
            RhythmMinigameManager.singleton.StartMinigame();
            timerAnimator.SetTrigger("End");
        }
    }

    void OnDisable()
    {
        RhythmEvents.OnNoteHittable -= NoteHittable;
    }

    public void StartTimer()
    {
        timerAnimator.SetTrigger("Start");
    }

    public void ShowLostPanel()
    {
        lostPanel.SetActive(true);
    }
    public void ShowFinishPanel(float hitPercentage, int rankValue)
    {
        hitText.text = string.Format("{0:0.0}%", hitPercentage);
        rankText.text = string.Format("{0}/5", rankValue);
        this.rankValue = rankValue;
        finishPanel.SetActive(true);
    }

    public void GiveUpMinigame()
    {
        Time.timeScale = 1f;
        if(player != null)
        {
            player.loadPointName = "Saida minigame";
            player.StopPlayingMinigame();
        }
        //Acrescentar uma variavel de dialogo que Porã desistiu da atividade
        InstancesManager.singleton.GetInMemoryVariableStorage().SetValue("$farmDone", true);
        InstancesManager.singleton.GetInMemoryVariableStorage().SetValue("$farmCultiveGiveUp", true);
        //Atualiza o turno do dia
        InstancesManager.singleton.GetDayManager().UpdateDayShift();
        //Aciona o evento de quest que completou o minigame
        InstancesManager.singleton.GetQuestEvents().PiataFarmCompleted();
        //Voltar para a fazenda
        InstancesManager.singleton.GetLevelLoaderInstance().LoadNextLevel("FazendaPiata", 0);
    }
    
    public void RestartMinigame()
    {
        Time.timeScale = 1f;
        InstancesManager.singleton.GetLevelLoaderInstance().LoadNextLevel("PiataMinigame", 1);
    }

    public void FinishMinigame()
    {
        Time.timeScale = 1f;
        if (player != null)
        {
            player.loadPointName = "Saida minigame";
            player.StopPlayingMinigame();
        }
        //Seta a variavel dizendo que ele terminou a atividade
        //E coloca a nota que ele tirou na atividade
        InstancesManager.singleton.GetInMemoryVariableStorage().SetValue("$farmDone", true);
        InstancesManager.singleton.GetInMemoryVariableStorage().SetValue("$farmCultiveFinish", true);
        InstancesManager.singleton.GetInMemoryVariableStorage().SetValue("$farmCultiveRank", rankValue);
        //Aciona o evento de quest que completou o minigame
        InstancesManager.singleton.GetQuestEvents().PiataFarmCompleted();
        //Atualiza o turno do dia
        InstancesManager.singleton.GetDayManager().UpdateDayShift();
        //Voltar para a fazenda
        InstancesManager.singleton.GetLevelLoaderInstance().LoadNextLevel("FazendaPiata", 0);
        
    }

    private void NoteHittable(string noteName)
    {
        if (noteName.Equals("seeds"))
        {
            piataAnimator.SetTrigger("Seeds");
            return;
        }

        if (noteName.Equals("water"))
        {
            piataAnimator.SetTrigger("Water");
            return;
        }
    }
}