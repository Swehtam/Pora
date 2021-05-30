using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvents : MonoBehaviour
{
    public delegate void NPCEventHandler(string npcName);
    public static event NPCEventHandler OnNPCTalk;

    public delegate void PlaceEventHandler(string placeName);
    public static event PlaceEventHandler OnPlaceArrive;

    public delegate void SwimDodgeEventHandler(int difficulty, int maxDistance);
    public static event SwimDodgeEventHandler OnSwimDodgeCompleted;

    public delegate void NewQuestEventHandler(Quest quest);
    public static event NewQuestEventHandler OnNewQuest;

    public delegate void NewQuestGoalEventHandler(QuestGoal questGoal);
    public static event NewQuestGoalEventHandler OnNewQuestGoal;
    public static void NPCTalk(string npcName)
    {
        if (OnNPCTalk != null)
            OnNPCTalk(npcName);
    }

    public static void PlaceArrive(string placeName)
    {
        if (OnPlaceArrive != null)
            OnPlaceArrive(placeName);
    }

    public static void SwimDodgeCompleted(int difficulty, int maxDistance)
    {
        if (OnSwimDodgeCompleted != null)
            OnSwimDodgeCompleted(difficulty, maxDistance);
    }

    public static void GotNewQuest(Quest quest)
    {
        if (OnNewQuest != null)
            OnNewQuest(quest);
    }

    public static void UpdateQuestGoal(QuestGoal questGoal)
    {
        if (OnNewQuestGoal != null)
            OnNewQuestGoal(questGoal);
    }
}
