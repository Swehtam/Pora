using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Quest : MonoBehaviour
{
    public int id;
    public string title;
    public string description;
    public bool isCompleted;
    public bool isActive;

    [System.Serializable]
    public struct GoalsVariables
    {
        public GoalType goalType;

        public string description;

        public int currentAmount;

        public int requiredAmount;

        [Header("Se o tipo de objetivo for do minigame Nado a Desvio")]
        public int maxDistance;
        public int difficulty;

        [Header("Se o tipo de objetivo for de ir para um local")]
        public string placeName;

        [Header("Se o tipo de objetivo for de falar com NPC")]
        public string npcName;
    }
    public GoalsVariables[] arrayGoalsVariables;
    private bool isAvaliable = true;
    private QuestsManager questsManager;

    [Header("Optativo")]
    public List<int> completedQuestsID = new List<int>();

    void Start()
    {
        questsManager = InstancesManager.singleton.GetQuestsManager();

        //Verifica se quest já foi completada anteriormente
        if (QuestAlreadyCompleted())
        {
            this.enabled = false;
            isCompleted = true;
            isAvaliable = false;
            return;
        }

        //Verifica se quest já está ativa
        if (QuestActivated())
        {
            this.enabled = false;
            isActive = true;
            isAvaliable = false;
            return;
        }

        //Verifica se essa quest pode ser realizada agora
        if (AllConditionsAreCompleted() == false)
        {
            //Seta que essa quest não ser realizada agora,
            //pois não cumpre com os requisitos
            this.enabled = false;
            isAvaliable = false;
            return;
        }

        print("Quest avaliavel: " + id);
    }

    [YarnCommand("startQuest")]
    public void StartQuest(string questIdString)
    {
        print("achou");

        if (int.TryParse(questIdString, out var questId) == false)
        {
            Debug.LogErrorFormat($"<<startQuest>> failed to parse quest id {questIdString}");
            return;
        }

        if (isAvaliable && (id == questId))
        {
            //Informa ao administrador de missões que essa aqui começou
            questsManager.AddQuest(this);
        }
    }

    /// <summary>
    /// Metodo para verficar se todas os requisitos presentes na quest
    /// foram cumpridos
    /// </summary>
    /// <returns></returns>
    private bool AllConditionsAreCompleted()
    {
        //Verificação se quese quest pode ser iniciada ou não
        if (completedQuestsID.Count > 0)
        {
            if (questsManager.CheckIfQuestsWereCompleted(completedQuestsID) == false)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Metodo para verficar se essa quest já foi completada anteriormente
    /// </summary>
    /// <returns></returns>
    private bool QuestAlreadyCompleted()
    {
        return questsManager.CheckIfQuestWasCompleted(id);
    }

    private bool QuestActivated()
    {
        return questsManager.CheckIfQuestIsActive(id);
    }
}