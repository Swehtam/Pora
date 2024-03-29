﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Yarn.Unity;

/* Classe criada baseado no padrão de projeto Singleton.
 * Seu intuito é guardar e fornecer instâncias de classes únicas presentes em cada scene do jogo.
 * Diminuindo o número de chamadas do método FindObjectOfType.
 * Caso a instância não exista no singleton, o mesmo irá buscar essa instância e salvar em seu objeto estático.
 */
public class InstancesManager : MonoBehaviour
{
    public static InstancesManager singleton;
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private PlayerSettings playerSettings;
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private InMemoryVariableStorage inMemoryVariableStorage;
    [SerializeField] private NodeVisitedTracker nodeVisitedTracker;
    [SerializeField] private YarnPlacesManager yarnPlacesManager;
    [SerializeField] private DayManager dayManager;
    [SerializeField] private QuestsManager questsManager;
    [SerializeField] private QuestEvents questEvents;
    [SerializeField] private NPCManager npcManager;
    [SerializeField] private MinigamesManager minigamesManager;
    [SerializeField] private SaveSystem saveSystem;
    private GameObject player;
    private UIManager uIManager;

    void Awake()
    {
        if (singleton != null)
            Destroy(singleton);
        else
            singleton = this;

        DontDestroyOnLoad(this);
    }

    //Método responsável por retornar o GameObject do jogador para quem solicitar
    //Caso o GameObject não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por game)
    public GameObject GetPlayerInstance()
    {
        if (player == null)
        {
            var aux = FindObjectOfType<PlayerController>();

            //Se não achar é pq essa é a primeira scene a ser carregada no game, então o GameController irá criar o prefab na scene
            if(aux != null)
                player = aux.gameObject;
        }
            
        return player;
    }

    //Método responsável por retornar o DialogueRunner do _preload para quem solicitar
    //Caso o DialogueRunner não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por cena)
    public DialogueRunner GetDialogueRunnerInstance()
    {
        if (dialogueRunner == null)
            dialogueRunner = FindObjectOfType<DialogueRunner>();

        return dialogueRunner;
    }

    //Método responsável por retornar o PlayerSettings do _preload para quem solicitar
    //Caso o PlayerSettings não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por game)
    public PlayerSettings GetPlayerSettingsInstance()
    {
        if (playerSettings == null)
            playerSettings = FindObjectOfType<PlayerSettings>();

        return playerSettings;
    }

    //Método responsável por retornar o LevelLoader do _preload para quem solicitar
    //Caso o LevelLoader não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por game)
    public LevelLoader GetLevelLoaderInstance()
    {
        if (levelLoader == null)
            levelLoader = FindObjectOfType<LevelLoader>();

        return levelLoader;
    }

    //Método responsável por retornar o UIManager de cada cena para quem solicitar
    //Caso o UIManager não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por cena)
    public UIManager GetUIManager()
    {
        if (uIManager == null)
            uIManager = FindObjectOfType<UIManager>();

        return uIManager;
    }

    //Método responsável por retornar o InMemoryVariableStorage do _preload para quem solicitar
    //Caso o InMemoryVariableStorage não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por game)
    public InMemoryVariableStorage GetInMemoryVariableStorage()
    {
        if (inMemoryVariableStorage == null)
            inMemoryVariableStorage = FindObjectOfType<InMemoryVariableStorage>();

        return inMemoryVariableStorage;
    }

    //Método responsável por retornar o NodeVisitedTracker do _preload para quem solicitar
    //Caso o NodeVisitedTracker não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por game)
    public NodeVisitedTracker GetNodeVisitedTracker()
    {
        if (nodeVisitedTracker == null)
            nodeVisitedTracker = FindObjectOfType<NodeVisitedTracker>();

        return nodeVisitedTracker;
    }

    //Método responsável por retornar o YarnPlacesManager do _preload para quem solicitar
    //Caso o YarnPlacesManager não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por game)
    public YarnPlacesManager GetYarnPlacesManager()
    {
        if (yarnPlacesManager == null)
            yarnPlacesManager = FindObjectOfType<YarnPlacesManager>();

        return yarnPlacesManager;
    }

    //Método responsável por retornar o DayManager do _preload para quem solicitar
    //Caso o DayManager não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por game)
    public DayManager GetDayManager()
    {
        if (dayManager == null)
            dayManager = FindObjectOfType<DayManager>();

        return dayManager;
    }

    //Método responsável por retornar o QuestsManager do _preload para quem solicitar
    //Caso o QuestsManager não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por game)
    public QuestsManager GetQuestsManager()
    {
        if (questsManager == null)
            questsManager = FindObjectOfType<QuestsManager>();

        return questsManager;
    }

    //Método responsável por retornar o QuestEvents do _preload para quem solicitar
    //Caso o QuestEvents não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por game)
    public QuestEvents GetQuestEvents()
    {
        if (questEvents == null)
            questEvents = FindObjectOfType<QuestEvents>();

        return questEvents;
    }

    //Método responsável por retornar o NPCManager do cena atual para quem solicitar
    //Caso o NPCManager não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por cena)
    public NPCManager GetNPCManager()
    {
        if (npcManager == null)
            npcManager = FindObjectOfType<NPCManager>();

        return npcManager;
    }

    //Método responsável por retornar o MinigamesManager do cena atual para quem solicitar
    //Caso o MinigamesManager não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por jogo)
    public MinigamesManager GetMinigamesManager()
    {
        if (minigamesManager == null)
            minigamesManager = FindObjectOfType<MinigamesManager>();

        return minigamesManager;
    }

    //Método responsável por retornar o SaveSystem do cena atual para quem solicitar
    //Caso o SaveSystem não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por jogo)
    public SaveSystem GetSaveSystem()
    {
        if (saveSystem == null)
            saveSystem = FindObjectOfType<SaveSystem>();

        return saveSystem;
    }
}
