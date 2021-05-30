using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsManager : MonoBehaviour
{
    //private static Dictionary<int, Quest> activeQuests = new Dictionary<int, Quest>();
    private static Quest activeQuest = null;
    private static Dictionary<int, Quest> completedQuests = new Dictionary<int, Quest>();
    public static bool newQuest = false;

    // Start is called before the first frame update
    void Start()
    {
        LoadQuests();
    }

    /// <summary>
    /// Metodo estatico para completar uma quest presente nas quests ativas
    /// </summary>
    /// <param name="id"></param>
    public static void AddCompleteQuest(int id)
    {
        //Checa se o dicionário de quests ativas tem a quest com o id do parametro
        /*if (activeQuests.ContainsKey(id)){
            //Caso tenha pegar a quest em questão
            Quest completedQuest = activeQuests[id];
            //Remover essa quest do dicionário de quests ativas
            activeQuests.Remove(id);

            //Por fim adicionar no dicionario de quests completas
            completedQuests.Add(id, completedQuest);
        }
        //Caso não tenha informar que não foi possivel achar a quest
        else
        {
            Debug.LogErrorFormat($"failed to find active quest id: {id}.");
        }*/
        if(activeQuest.id == id)
        {
            completedQuests.Add(id, activeQuest);
            activeQuest = null;
        }
        else
        {
            Debug.LogErrorFormat($"failed to find active quest id: {id}.");
        }
    }

    /// <summary>
    /// Metodo para adicionar uma nova quest nas quests ativas
    /// </summary>
    /// <param name="id"></param>
    /// <param name="quest"></param>
    public static void AddQuest(int id, Quest quest)
    {
        /*if (activeQuests.ContainsKey(id))
        {
            Debug.LogErrorFormat($"quest with id: {id} already exists.");
        }
        else
        {
            //Adiciona a quest nesse dicionario de quests ativas
            activeQuests.Add(id, quest);
        }*/
        activeQuest = quest;
        QuestEvents.GotNewQuest(quest);
    }

    public static Quest GetActiveQuest()
    {
        return activeQuest;
    }

    /// <summary>
    /// Metodo para verificar se todas as quests na lista de quests foram completadas anteriormente.
    /// </summary>
    /// <param name="questsID"></param>
    /// <returns></returns>
    public static bool CheckIfQuestsWereCompleted(List<int> questsID)
    {
        //Se todas foram true significa que essas quests já foram completadas
        return questsID.TrueForAll(g => completedQuests.ContainsKey(g));
    }

    /// <summary>
    /// Metodo para verificar se a quest já foi completada anteriormente
    /// </summary>
    /// <param name="questID"></param>
    /// <returns></returns>
    public static bool CheckIfQuestWasCompleted(int questID)
    {
        return completedQuests.ContainsKey(questID);
    }

    /// <summary>
    /// Metodo a ser chamado para carregar todas as quests que foram completadas e ativadas.
    /// </summary>
    private void LoadQuests()
    {

    }
}
