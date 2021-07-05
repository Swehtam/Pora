using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvents : MonoBehaviour
{
    public delegate void NPCEventHandler(string npcName);
    public event NPCEventHandler OnNPCTalk;

    public delegate void PlaceEventHandler(string placeName);
    public event PlaceEventHandler OnPlaceArrive;

    public delegate void SwimDodgeEventHandler(int difficulty, int maxDistance);
    public event SwimDodgeEventHandler OnSwimDodgeCompleted;

    public delegate void NewQuestEventHandler();
    public event NewQuestEventHandler OnNewQuest;

    public delegate void NewQuestGoalEventHandler(QuestGoal questGoal);
    public event NewQuestGoalEventHandler OnNewQuestGoal;
    
    public void NPCTalk(string npcName)
    {
        OnNPCTalk?.Invoke(npcName);
    }

    public void PlaceArrive(string placeName)
    {
        OnPlaceArrive?.Invoke(placeName);
    }

    public void SwimDodgeCompleted(int difficulty, int maxDistance)
    {
        OnSwimDodgeCompleted?.Invoke(difficulty, maxDistance);
    }

    public void GotNewQuest()
    {
        OnNewQuest?.Invoke();
    }

    public void UpdateQuestGoal(QuestGoal questGoal)
    {
        OnNewQuestGoal?.Invoke(questGoal);
    }
}
