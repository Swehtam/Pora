using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveQuest : MonoBehaviour
{
    public int id;
    public string title;
    public string description;
    public bool isCompleted;
    public bool isActive;

    private List<QuestGoal> goals = new List<QuestGoal>();
    //Variaveis para saber qual o goal atual
    private QuestGoal currentGoal;
    private int currentGoalValue = -1;

    public void StarQuest(Quest quest)
    {
        id = quest.id;
        title = quest.title;

        //Checa se não teve erro e a quest possui objetivos
        if (quest.arrayGoalsVariables.Length != 0)
        {
            //Adiciona cada um dos goals na lista de goals da quest
            foreach (Quest.GoalsVariables g in quest.arrayGoalsVariables)
            {
                switch (g.goalType)
                {
                    //Caso seja do objetivo do minigame de nado a desvio
                    case GoalType.SwimDodgeMinigame:
                        goals.Add(new SwimDodgeMinigameGoal(this, g.difficulty, g.maxDistance, g.description, false, g.currentAmount, g.requiredAmount));
                        break;
                    case GoalType.BiologyMinigame:
                        goals.Add(new BiologyClassMinigameGoal(this, g.description, false, g.currentAmount, g.requiredAmount));
                        break;
                    case GoalType.PiataFarm:
                        goals.Add(new PiataFarmMinigameGoal(this, g.description, false, g.currentAmount, g.requiredAmount));
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

        //Diz que a quest ta ativa
        isActive = true;
        //Ativa o primeiro objetivo da quest
        currentGoalValue++;
        currentGoal = goals[currentGoalValue];
        currentGoal.Init();
        description = currentGoal.Description;
    }

    /// <summary>
    /// Metodo chamado por um goal quando ele é completado
    /// </summary>
    public void CheckGoals()
    {
        //Se o objetivo atual for igual ao total de objetivos então acabou
        if ((currentGoalValue + 1) == goals.Count)
        {
            isCompleted = true;
            isActive = false;
            //Pega o Quest Manager e completa a missão
            InstancesManager.singleton.GetQuestsManager().AddCompleteQuest(id);
            //a variavel activeQuest no QuestManager agora é null, então pode destruir essa missão
            QuestCompleted();
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
        InstancesManager.singleton.GetQuestEvents().UpdateQuestGoal(currentGoal);
    }

    /// <summary>
    /// Destroi essa quest quando for completada, pois não precisa mais de uma referencia direta a ela
    /// </summary>
    private void QuestCompleted()
    {
        Destroy(this);
    }
}
