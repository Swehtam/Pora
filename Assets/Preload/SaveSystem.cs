using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Yarn.Unity;

public class SaveSystem : MonoBehaviour
{
    public GameSave gameSave;
    public bool fileLoaded;

    void Start()
    {
        SaveLoadEvents.OnGameSave += SaveData;
        SaveLoadEvents.OnGameLoad += LoadData;
        gameSave = new GameSave();
        LoadXMLFile();
    }

    private void OnDisable()
    {
        SaveLoadEvents.OnGameSave -= SaveData;
        SaveLoadEvents.OnGameLoad -= LoadData;
    }

    public void SaveData()
    {
        string path = Application.persistentDataPath + "/save.data";
        XmlSerializer serializer = new XmlSerializer(typeof(GameSave));
        TextWriter writer = new StreamWriter(path);
        gameSave.SaveData();

        serializer.Serialize(writer, gameSave);
        writer.Close();
        Debug.Log("Jogo salvo com sucesso! Caminho: " + path);
    }

    public void LoadXMLFile()
    {
        string path = Application.persistentDataPath + "/save.data";
        if (File.Exists(path))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameSave));
            
            //Se tiver algum Node, Atributo ou Elemento diferente então chama os metodo para lidar com o problema e excluir o arquivo
            serializer.UnknownNode += new XmlNodeEventHandler(Serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(Serializer_UnknownAttribute);
            serializer.UnknownElement += new XmlElementEventHandler(Serializer_UnknownElement);

            //FileStream para abrir o arquivo
            FileStream fs = new FileStream(path, FileMode.Open);
            //Carrega o arquivo
            gameSave = (GameSave)serializer.Deserialize(fs);
            fs.Close();

            fileLoaded = true;
            //Carrega as variaveis de PlayerSetting
            PlayerSettings playerS = InstancesManager.singleton.GetPlayerSettingsInstance();
            playerS.LoadPlayerSettings(gameSave.gameFinished);
        }
        else
        {
            Debug.LogError("Arquivo salvo não encontrado no caminho: " + path);
        }
    }

    public void LoadData()
    {
        //gameSave.PrintValuesSaved();
        gameSave.LoadData();
        Debug.Log("Jogo carregado com sucesso!");
    }

    /// <summary>
    /// Metodo para deletar arquivo e objeto de dados salvo, caso alguém altere o arquivo de dados salvos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Serializer_UnknownNode (object sender, XmlNodeEventArgs e)
    {
        Debug.LogError("Unknown Node:" + e.Name + "\t" + e.Text);
        //string path = Application.persistentDataPath + "/save.data";
        //File.Delete(path);
        gameSave = null;
    }

    /// <summary>
    /// Metodo para deletar arquivo e objeto de dados salvo, caso alguém altere o arquivo de dados salvos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Serializer_UnknownAttribute (object sender, XmlAttributeEventArgs e)
    {
        XmlAttribute attr = e.Attr;
        Debug.LogError("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
        //string path = Application.persistentDataPath + "/save.data";
        //File.Delete(path);
        gameSave = null;
    }

    /// <summary>
    /// Metodo para deletar arquivo e objeto de dados salvo, caso alguém altere o arquivo de dados salvos
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Serializer_UnknownElement(object sender, XmlElementEventArgs e)
    {
        Debug.LogError("Unknown Element" + "\n\t" + e.Element.Name + " " + e.Element.InnerXml +
                       "\n\t LineNumber: " + e.LineNumber + "\n\t LinePosition: " + e.LinePosition);
        //string path = Application.persistentDataPath + "/save.data";
        //File.Delete(path);
        gameSave = null;
    }
}

[XmlRootAttribute("GameSave", IsNullable = false)]
public class GameSave
{
    //Nós visitados
    public string[] visitedDialogueNodes;

    //Dia do jogo
    public int gameday;

    //Yarn Places completados
    public string[] yarnPlacesDone;

    //Variaveis do minigames
    public MinigamesSave minigamesSave;

    //Quests completas
    public int[] completedQuests;

    //Variveis do dialogo
    public DialogueMemoryVariables dialogueMemoryVariables;

    public bool gameFinished;

    public void LoadData()
    {
        NodeVisitedTracker nodesVT = InstancesManager.singleton.GetNodeVisitedTracker();
        DayManager dayM = InstancesManager.singleton.GetDayManager();
        YarnPlacesManager yarnPM = InstancesManager.singleton.GetYarnPlacesManager();
        MinigamesManager minigamesM = InstancesManager.singleton.GetMinigamesManager();
        QuestsManager questM = InstancesManager.singleton.GetQuestsManager();
        InMemoryVariableStorage inMemoryVS = InstancesManager.singleton.GetInMemoryVariableStorage();

        //Carrega os nós dos dialogos que foram salvos
        nodesVT.LoadNodesVisited(visitedDialogueNodes);

        //Carrega o dia que foi salvo
        dayM.LoadDay(gameday);

        //Carrega os Yarn Places completos
        yarnPM.LoadYarnPlaceDone(yarnPlacesDone);

        //Carrega as variaveis dos Minigames
        minigamesM.LoadMinigamesVariables(minigamesSave);

        //Carrega as quests completadas
        questM.LoadQuests(completedQuests);

        //Carrega as variaveis de dialogo
        inMemoryVS.LoadVariables(dialogueMemoryVariables);
    }

    public void SaveData()
    {
        NodeVisitedTracker nodesVT = InstancesManager.singleton.GetNodeVisitedTracker();
        DayManager dayM = InstancesManager.singleton.GetDayManager();
        YarnPlacesManager yarnPM = InstancesManager.singleton.GetYarnPlacesManager();
        MinigamesManager minigamesM = InstancesManager.singleton.GetMinigamesManager();
        QuestsManager questM = InstancesManager.singleton.GetQuestsManager();
        InMemoryVariableStorage inMemoryVS = InstancesManager.singleton.GetInMemoryVariableStorage();
        PlayerSettings playerS = InstancesManager.singleton.GetPlayerSettingsInstance();

        //Salva os nós dos dialogos
        visitedDialogueNodes = nodesVT.SaveNodesVisited();

        //Salva o dia
        gameday = dayM.GetDay();

        //Salva os Yarn Places completos
        yarnPlacesDone = yarnPM.SaveYarnPlaceDone();

        //Salva as variaveis dos minigames
        minigamesSave = minigamesM.SaveMinigamesVariables();

        //Salva as quests que já foram completadas
        completedQuests = questM.SaveQuests();

        //Salva as variaveis de dialogo
        dialogueMemoryVariables = inMemoryVS.SaveVariables();

        //Salva o player Settings
        gameFinished = playerS.SavePlayerSettings();
    }

    public void PrintValuesSaved()
    {
        Debug.Log("Dialogos visitados: ");
        foreach(string v in visitedDialogueNodes)
        {
            Debug.Log("\t - " + v);
        }

        Debug.Log("Dia do jogo: " + gameday);

        Debug.Log("Yarn Places visitados: ");
        foreach (string y in yarnPlacesDone)
        {
            Debug.Log("\t - " + y);
        }

        Debug.Log("Quests finalizadas: ");
        foreach (int q in completedQuests)
        {
            Debug.Log("\t - " + q);
        }

        Debug.Log("Variaveis de dialogos: ");
        Debug.Log("\t - Variaveis floats: ");
        for(int i = 0; i < dialogueMemoryVariables.floatVarName.Length; i++)
        {
            Debug.Log("\t\t - " + dialogueMemoryVariables.floatVarName[i] + ": " + dialogueMemoryVariables.floatVar);
        }

        Debug.Log("\t - Variaveis bools: ");
        for (int i = 0; i < dialogueMemoryVariables.boolVarName.Length; i++)
        {
            Debug.Log("\t\t - " + dialogueMemoryVariables.boolVarName[i] + ": " + dialogueMemoryVariables.boolVar);
        }

        Debug.Log("\t - Variaveis strings: ");
        for (int i = 0; i < dialogueMemoryVariables.stringVarName.Length; i++)
        {
            Debug.Log("\t\t - " + dialogueMemoryVariables.stringVarName[i] + ": " + dialogueMemoryVariables.stringVar);
        }
    }
}

public class MinigamesSave
{
    //[XmlAttribute]
    //public string name = "Desvio a Nado";
    public int distanceMax;
    public int swimDodgeDifficulty;
}