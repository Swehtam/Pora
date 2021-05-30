using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGoal
{
    public Quest Quest { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
    public int CurrentAmount { get; set; }
    public int RequiredAmount { get; set; }

    public virtual void Init()
    {
        //Faz alguma coisa
    }

    public void Evaluate()
    {
        if (CurrentAmount >= RequiredAmount)
        {
            Complete();
        }
    }

    public void Complete()
    {
        this.Quest.CheckGoals();
        Completed = true;
    }
}

public class SwimDodgeMinigameGoal : QuestGoal
{
    public int Difficulty { get; set; }
    public int MaxDistance { get; set; }
    public SwimDodgeMinigameGoal(Quest quest, int difficulty, int maxDistance, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.Quest = quest;
        this.Difficulty = difficulty;
        this.MaxDistance = maxDistance;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        QuestEvents.OnSwimDodgeCompleted += SwimDodgeCompleted;
    }

    void SwimDodgeCompleted(int difficulty, int maxDistance)
    {
        if (difficulty >= this.Difficulty && maxDistance >= this.MaxDistance)
        {
            this.CurrentAmount++;
            Evaluate();
        }

        if (Completed)
        {
            QuestEvents.OnSwimDodgeCompleted -= SwimDodgeCompleted;
        }
    }
}

public class TalkToGoal : QuestGoal
{
    public string NPCName { get; set; }
    public TalkToGoal(Quest quest, string npcName, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.Quest = quest;
        this.NPCName = npcName;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        QuestEvents.OnNPCTalk += NPCTalk;
    }

    void NPCTalk(string npcName)
    {
        if (npcName.Equals(NPCName))
        {
            this.CurrentAmount++;
            Evaluate();
        }

        if (Completed)
        {
            QuestEvents.OnNPCTalk -= NPCTalk;
        }
    }
}

public class GoToGoal : QuestGoal
{
    public string PlaceName { get; set; }
    public GoToGoal(Quest quest, string placeName, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.Quest = quest;
        this.PlaceName = placeName;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        QuestEvents.OnPlaceArrive += PlaceArrive;
    }

    void PlaceArrive(string placeName)
    {
        if (placeName.Equals(PlaceName))
        {
            this.CurrentAmount++;
            Evaluate();
        }

        if (Completed)
        {
            QuestEvents.OnPlaceArrive -= PlaceArrive;
        }
    }
}

public enum GoalType
{
    SwimDodgeMinigame,
    Talking,
    GoTo
}
