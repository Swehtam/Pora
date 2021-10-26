using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System;

public class NPCSceneSpawner : MonoBehaviour
{
    public bool onSceneDefault = true;

    [SerializeField] private YarnNPC yarnNPC;

    [Serializable]
    public struct EventsVariables
    {
        /// <summary>
        /// O nome do evento para posicionar o NPC.
        /// </summary>
        /// <remarks>
        /// Lembrar de incluir o prefixo `$` nas variaveis.
        /// </remarks>
        public string name;

        /// <summary>
        /// A posi��o que o NPC vai ficar caso o evento esteja ocorrendo.
        /// </summary>
        public Transform transform;
    }

    [Serializable]
    public struct DontSpawnVariables
    {
        /// <summary>
        /// O nome da variavel para nao spawnar o NPC
        /// </summary>
        /// <remarks>
        /// Lembrar de incluir o prefixo `$` nas variaveis.
        /// </remarks>
        public string name;
    }

    [Serializable]
    public struct NPCPositions
    {
        /// <summary>
        /// Lugares onde o NPC vai spawnar ou andar at�.
        /// </summary>
        public Transform transform;
    }

    [Header("Todas as variaveis abaixo s�o OPCIONAIS")]
    [Header("Spawn Evento")]
    public EventsVariables[] arrayEventsVariables;

    [Header("N�o Spawnar")]
    public DontSpawnVariables[] arrayDontSpawnVariables;

    [Header("Posi��es do NPC")]
    public NPCPositions[] arrayNPCPositions;

    private bool isNPCSpawning = false;
    private InMemoryVariableStorage inMemoryVariableStorage;

    void Start()
    {
        //Caso o NPC esteja sendo spawnado atrav�s do dialgo, ignorar todo o resto do metodo
        if (isNPCSpawning)
        {
            isNPCSpawning = false;
            return;
        }            

        //Pega a classe que v� as variaveis na memoria
        if(inMemoryVariableStorage == null)
            inMemoryVariableStorage = InstancesManager.singleton.GetInMemoryVariableStorage();

        //Caso o default seja pra ser ativo, ent�o ativar, caso n�o desativa
        gameObject.SetActive(onSceneDefault);

        //Ap�s isso, caso tenha algum evento que o NPC participe colocar na posi��o correta
        if (arrayEventsVariables.Length > 0)
            CheckEventsVariables();

        if (arrayDontSpawnVariables.Length > 0)
            CheckDontSpawnVariables();
    }

    /// <summary>
    /// Metodo para spawnar o NPC baseado no ID da posi��o
    /// </summary>
    /// <param name="positionID"></param>
    public void SpawnNPC(int positionID)
    {
        isNPCSpawning = true;
        //Se a quantidade de posi��es for maior do ID ent�o � porque existe
        if(arrayNPCPositions.Length > positionID)
        {
            gameObject.transform.position = arrayNPCPositions[positionID].transform.position;
        }
        else
        {
            Debug.LogErrorFormat($"<<showNPC>> failed to find position id: {positionID}");
        }
    }

    [YarnCommand("moveTo")]
    public void MoveTo(string stringPositionID, string stringTimeToMove)
    {
        if (int.TryParse(stringPositionID, out var positionID) == false)
        {
            Debug.LogErrorFormat($"<<moveTo>> failed to parse position id {stringPositionID}");
            return;
        }

        if (float.TryParse(stringTimeToMove, 
                                System.Globalization.NumberStyles.AllowDecimalPoint,
                                System.Globalization.CultureInfo.InvariantCulture, 
                                out var timeToMove) == false)
        {
            Debug.LogErrorFormat($"<<moveTo>> failed to parse time to reach target{stringTimeToMove}");
            return;
        }

        //Se a quantidade de posi��es for maior do ID ent�o � porque existe
        if (arrayNPCPositions.Length > positionID)
        {
            yarnNPC.MoveTo(arrayNPCPositions[positionID].transform.position, timeToMove);
        }
        else
        {
            Debug.LogErrorFormat($"<<showNPC>> failed to find position id: {positionID}");
        }
    }

    /// <summary>
    /// Metodo para verificar se tem algum evento acontecendo
    /// Caso esteja, ent�o posicionar o NPC na posi��o destinada para o evento
    /// </summary>
    private void CheckEventsVariables()
    {
        foreach (EventsVariables ev in arrayEventsVariables)
        {
            //Pega a variavel se existir,
            //Se nao existir vai vir null, ou seja tipo diferente, ent�o nao precisa comparar os valores
            Yarn.Value memoryValue = inMemoryVariableStorage.GetValue(ev.name);
            //Se n�o existir a variavel ent�o continua
            if (memoryValue == null)
                continue;

            if (memoryValue.type == Yarn.Value.Type.Bool)
            {
                string stringValue = memoryValue.AsBool.ToString();
                if (stringValue.Equals("True"))
                {
                    //Ativa o NPC, pois tem a chance dele n estar ativo nessa cena e depois o posiciona
                    gameObject.SetActive(true);
                    transform.position = ev.transform.position;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Metodo para verificar se alguma das variaveis da lista s�o verdadeiras
    /// Caso alguma delas seja, ent�o n�o spawnar o NPC na cena
    /// </summary>
    private void CheckDontSpawnVariables()
    {
        foreach(DontSpawnVariables ds in arrayDontSpawnVariables)
        {
            //Pega a variavel se existir,
            //Se nao existir vai vir null, ou seja tipo diferente, ent�o nao precisa comparar os valores
            Yarn.Value memoryValue = inMemoryVariableStorage.GetValue(ds.name);
            //Se n�o existir a variavel ent�o continua
            if (memoryValue == null)
                continue;

            if (memoryValue.type == Yarn.Value.Type.Bool)
            {
                string stringValue = memoryValue.AsBool.ToString();
                if (stringValue.Equals("True"))
                {
                    //N�o spawnar, pois esse NPC j� apareceu em outra cena antes.
                    gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
}
