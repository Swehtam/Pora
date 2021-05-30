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

    private List<QuestGoal> goals = new List<QuestGoal>();
    //Variaveis para saber qual o goal atual
    private QuestGoal currentGoal;
    private int currentGoalValue = -1;
    private bool isAvaliable = true;

    [Header("Optativo")]
    public List<int> completedQuestsID = new List<int>();

    void Start()
    {
        //Verifica se quest já foi completada anteriormente
        if (QuestAlreadyCompleted())
        {
            this.enabled = false;
            isCompleted = true;
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

        //Checa se não teve erro e a quest possui objetivos
        if(arrayGoalsVariables.Length != 0)
        {
            //Adiciona cada um dos goals na lista de goals da quest
            foreach(GoalsVariables g in arrayGoalsVariables)
            {
                switch (g.goalType)
                {
                    //Caso seja do objetivo do minigame de nado a desvio
                    case GoalType.SwimDodgeMinigame:
                        goals.Add(new SwimDodgeMinigameGoal(this, g.difficulty, g.maxDistance, g.description, false, g.currentAmount, g.requiredAmount));
                        break;
                    case GoalType.GoTo:
                        goals.Add(new GoToGoal(this, g.placeName, g.description, false, g.currentAmount, g.requiredAmount));
                        break;
                    case GoalType.Talking:
                        goals.Add(new TalkToGoal(this, g.npcName, g.description, false, g.currentAmount, g.requiredAmount));
                        break;
                    default:
                        Debug.LogErrorFormat($"Erro inesperado, GoalType: {g.goalType} não existe.");
                        return;
                }
            }
        }
        else
        {
            //Caso ocorra algum erro e a quest não possua objetivos
            Debug.LogErrorFormat($"Quest (id: {id} e titulo: {title}) não possui objetivos.");
            return;
        }
    }

    [YarnCommand("startQuest")]
    public void StartQuest(string questIdString)
    {
        if (float.TryParse(questIdString,
                                   System.Globalization.NumberStyles.AllowDecimalPoint,
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   out var questId) == false)
        {

            Debug.LogErrorFormat($"<<startQuest>> failed to parse quest id {questIdString}");
            return;
        }
        if (isAvaliable && (id == questId))
        {
            //Diz que a quest ta ativa
            isActive = true;
            //Ativa o primeiro objetivo da quest
            currentGoalValue++;
            currentGoal = goals[currentGoalValue];
            currentGoal.Init();
            description = currentGoal.Description;
            //Seta que essa quest está ativa
            QuestsManager.AddQuest(id, this);
        }
    }

    /// <summary>
    /// Metodo chamado por um goal quando ele é completado
    /// </summary>
    public void CheckGoals()
    {
        //Se o objetivo atual for igual ao total de objetivos então acabou
        if((currentGoalValue + 1) == goals.Count)
        {
            isCompleted = true;
            isActive = false;
            QuestsManager.AddCompleteQuest(id);
        }
        //Se não for atualizar o objetivo atual
        else
        {
            UpdateQuestGoal();
        }
    }

    public QuestGoal GetQuestGoal()
    {
        return currentGoal;
    }
    /// <summary>
    /// Metodo para atualizar o próximo objetivo da quest
    /// </summary>
    private void UpdateQuestGoal()
    {
        //Passa para o próximo goal
        currentGoalValue++;
        //Atualiza o novo goal
        currentGoal = goals[currentGoalValue];
        //Inicia o novo goal
        currentGoal.Init();
        //Atualiza a descrição da missão
        description = currentGoal.Description;
        //Aciona o evento de atualização do objetivo
        QuestEvents.UpdateQuestGoal(currentGoal);
    }

    /// <summary>
    /// Metodo para verficar se todas os requisitos presentes na quest
    /// foram cumpridos
    /// </summary>
    /// <returns></returns>
    public bool AllConditionsAreCompleted()
    {
        //Verificação se quese quest pode ser iniciada ou não
        if (completedQuestsID.Count > 0)
        {
            if (QuestsManager.CheckIfQuestsWereCompleted(completedQuestsID) == false)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Metodo para verficar se essa quest já foi completada anteriormente
    /// </summary>
    /// <returns></returns>
    public bool QuestAlreadyCompleted()
    {
        return QuestsManager.CheckIfQuestWasCompleted(id);
    }
}