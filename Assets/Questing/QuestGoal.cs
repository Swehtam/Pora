using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGoal
{
    public ActiveQuest Quest { get; set; }
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

    private QuestEvents questEvents;

    public SwimDodgeMinigameGoal(ActiveQuest quest, int difficulty, int maxDistance, string description, bool completed, int currentAmount, int requiredAmount)
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
        questEvents = InstancesManager.singleton.GetQuestEvents();
        questEvents.OnSwimDodgeCompleted += SwimDodgeCompleted;
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
            questEvents.OnSwimDodgeCompleted -= SwimDodgeCompleted;
        }
    }
}

public class BiologyClassMinigameGoal : QuestGoal
{
    private QuestEvents questEvents;

    public BiologyClassMinigameGoal(ActiveQuest quest, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.Quest = quest;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        questEvents = InstancesManager.singleton.GetQuestEvents();
        questEvents.OnBiologyClassCompleted += BiologyClassCompleted;
    }

    void BiologyClassCompleted()
    {
        this.CurrentAmount++;
        Evaluate();

        if (Completed)
        {
            questEvents.OnBiologyClassCompleted -= BiologyClassCompleted;
        }
    }
}

public class PiataFarmMinigameGoal : QuestGoal
{
    private QuestEvents questEvents;

    public PiataFarmMinigameGoal(ActiveQuest quest, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.Quest = quest;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        questEvents = InstancesManager.singleton.GetQuestEvents();
        questEvents.OnPiataFarmCompleted += PiataFarmCompleted;
    }

    void PiataFarmCompleted()
{
        this.CurrentAmount++;
        Evaluate();

        if (Completed)
        {
            questEvents.OnPiataFarmCompleted -= PiataFarmCompleted;
        }
    }
}

public class TalkToGoal : QuestGoal
{
    public string NPCName { get; set; }

    private QuestEvents questEvents;

    public TalkToGoal(ActiveQuest quest, string npcName, string description, bool completed, int currentAmount, int requiredAmount)
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
        questEvents = InstancesManager.singleton.GetQuestEvents();
        questEvents.OnNPCTalk += NPCTalk;
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
            questEvents.OnNPCTalk -= NPCTalk;
        }
    }
}

public class GoToGoal : QuestGoal
{
    public string PlaceName { get; set; }

    private QuestEvents questEvents;

    public GoToGoal(ActiveQuest quest, string placeName, string description, bool completed, int currentAmount, int requiredAmount)
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
        questEvents = InstancesManager.singleton.GetQuestEvents();
        questEvents.OnPlaceArrive += PlaceArrive;
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
            questEvents.OnPlaceArrive -= PlaceArrive;
        }
    }
}

public enum GoalType
{
    SwimDodgeMinigame,
    Talking,
    GoTo,
    BiologyMinigame,
    PiataFarm
}
