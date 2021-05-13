using UnityEngine;
using Yarn.Unity;
using System;

/// attached to the non-player characters, and stores the name of the Yarn
/// node that should be run when you talk to them.

public class YarnNPC : MonoBehaviour
{
    public string characterName = "";

    public string talkToNode = "";

    [Header("Optional")]
    public YarnProgram scriptToLoad;
    [Serializable]
    public struct EventsVariables
    {
        /// <summary>
        /// O nome do evento para posicionar o NPC.
        /// </summary>
        /// <remarks>
        /// Lembrar de inckuir o prefixo `$` nas variaveis.
        /// </remarks>
        public string name;

        /// <summary>
        /// A posição que o NPC vai ficar caso o evento esteja ocorrendo.
        /// </summary>
        public Transform transform;
    }
    public EventsVariables[] arrayEventsVariables;
    public bool onSceneDefault = true;

    private UIManager uIManager;

    void Start()
    {
        //Caso o default seja pra ser ativo, então ativar, caso não desativa
        gameObject.SetActive(onSceneDefault);

        //Após isso, caso tenha algum evento que o NPC participe colocar na posição correta
        if(arrayEventsVariables.Length > 0)
            CheckEventsVariables();

        if (scriptToLoad != null)
        {
            DialogueRunner dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
            dialogueRunner.Add(scriptToLoad);
        }

        uIManager = InstancesManager.singleton.GetUIManager();
    }

    //Quando o NPC encontrar o Player mostrar o botão para falar
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            uIManager.ShowTalkButton(this);
    }

    //Usado para saber se o player saio de perto do NPC
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            uIManager.HideTalkButton();
    }

    private void CheckEventsVariables()
    {
        //Pega a classe que vê as variaveis na memoria
        var inMemoryVariableStorage = InstancesManager.singleton.GetInMemoryVariableStorage();

        foreach (EventsVariables ev in arrayEventsVariables)
        {
            //Pega a variavel se existir,
            //Se nao existir vai vir null, ou seja tipo diferente, então nao precisa comparar os valores
            Yarn.Value memoryValue = inMemoryVariableStorage.GetValue(ev.name);
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
}
