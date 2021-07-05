using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsManager : MonoBehaviour
{
    //Variavel usada para realizar anima��o quandp recebe miss�o nova
    public bool newQuest = false;
    //GameObject do "_quests" para adicionar componente de ActiveQuest assim que tiver miss�o nova
    public GameObject _quests;
    //Lista de miss�es j� completadas para teste
    public bool isTesting = false;
    public List<int> testingQuestsID = new List<int>();    

    //private static Dictionary<int, Quest> activeQuests = new Dictionary<int, Quest>();
    private ActiveQuest activeQuest;
    private readonly Dictionary<int, bool> completedQuests = new Dictionary<int, bool>();
    private QuestEvents questEvents;

    // Start is called before the first frame update
    void Start()
    {
        questEvents = InstancesManager.singleton.GetQuestEvents();
        LoadQuests();
    }

    /// <summary>
    /// Metodo estatico para completar uma quest presente nas quests ativas
    /// </summary>
    /// <param name="id"></param>
    public void AddCompleteQuest(int id)
    {
        //Checa se o dicion�rio de quests ativas tem a quest com o id do parametro
        /*if (activeQuests.ContainsKey(id)){
            //Caso tenha pegar a quest em quest�o
            Quest completedQuest = activeQuests[id];
            //Remover essa quest do dicion�rio de quests ativas
            activeQuests.Remove(id);

            //Por fim adicionar no dicionario de quests completas
            completedQuests.Add(id, completedQuest);
        }
        //Caso n�o tenha informar que n�o foi possivel achar a quest
        else
        {
            Debug.LogErrorFormat($"failed to find active quest id: {id}.");
        }*/
        
        if(activeQuest.id == id)
        {
            completedQuests.Add(id, true);
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
    public void AddQuest(Quest quest)
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
        activeQuest = _quests.AddComponent(typeof(ActiveQuest)) as ActiveQuest;
        activeQuest.StarQuest(quest);
        questEvents.GotNewQuest();
    }

    public ActiveQuest GetActiveQuest()
    {
        //Isso aqui funciona para saber se a miss�o foi completada ou n�o
        return activeQuest;
    }

    /// <summary>
    /// Metodo para verificar se todas as quests na lista de quests foram completadas anteriormente.
    /// </summary>
    /// <param name="questsID"></param>
    /// <returns></returns>
    public bool CheckIfQuestsWereCompleted(List<int> questsID)
    {
        //Se todas foram true significa que essas quests j� foram completadas
        return questsID.TrueForAll(g => completedQuests.ContainsKey(g));
    }

    /// <summary>
    /// Metodo para verificar se a quest j� foi completada anteriormente
    /// </summary>
    /// <param name="questID"></param>
    /// <returns></returns>
    public bool CheckIfQuestWasCompleted(int questID)
    {
        return completedQuests.ContainsKey(questID);
    }

    public bool CheckIfQuestIsActive(int questID)
    {
        //Atualmente s� pode ter 1 quest por vez, mas quando puder ter mais modificar essa parte
        if(activeQuest != null)
        {
            if (activeQuest.id == questID)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Metodo a ser chamado para carregar todas as quests que foram completadas e ativadas.
    /// </summary>
    private void LoadQuests()
    {
        if (isTesting)
        {
            foreach(int questID in testingQuestsID)
            {
                completedQuests.Add(questID, true);
            }
        }
    }
}
