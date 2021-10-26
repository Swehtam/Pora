using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System;

public class LoadNewScene : MonoBehaviour
{
    public string scene;

    public string exitPoint;
   
    public enum DayShift
    {
        Nenhum = -1,
        /// <summary>
        /// Pode entrar nesse minigame se for manhã
        /// </summary>
        Manha = 0,
        /// <summary>
        /// Pode entrar nesse minigame se for tarde
        /// </summary>
        Tarde = 1,
        /// <summary>
        /// Pode entrar nesse minigame se for noite
        /// </summary>
        Noite = 2,
        /// <summary>
        /// Pode entrar nesse minigame se for manhã ou tarde
        /// </summary>
        Manha_Tarde = 3,
        /// <summary>
        /// Pode entrar nesse minigame se for manhã ou noite
        /// </summary>
        Manha_Noite = 4,
        /// <summary>
        /// Pode entrar nesse minigame se for tarde ou noite
        /// </summary>
        Tarde_Noite = 5
    }

    [Serializable]
    public struct CantEnterVariables
    {
        /// <summary>
        /// O nome da variavel para não poder mudar de cena.
        /// </summary>
        /// <remarks>
        /// Lembrar de incluir o prefixo `$` nas variaveis.
        /// </remarks>
        public string name;
    }

    [SerializeField] private bool isLoadingMiniGame = false;
    
    [Header("Opcional")]
    [Tooltip("Opcional - Turno do dia para poder entrar no minigame.")]
    [SerializeField] private DayShift dayShiftCondition = DayShift.Nenhum;
    [SerializeField] private YarnProgram scriptToLoad;
    [SerializeField] private string notAbleToEnterNode = "";
    [Tooltip("Variaveis para não poder mudar de cena")]
    public CantEnterVariables[] arrayCantEnterVariables;

    private PlayerController player;
    private DialogueRunner dialogueRunner;
    private bool isAbleToEnter = false;

    void Start()
    {
        //Usa o singleton para pegar a instância do player
        player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();
        
        //Se não tiver variaveis que impeçam o player de entrar na cena
        //Ou as variaveis não forem verdadeiras,
        //E se o turno do dia estiver correto, então pode entrar
        if (CheckCantEnterVariables() && CheckDayShift())
        {
            isAbleToEnter = true;
        }

        if (scriptToLoad != null)
        {
            dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
            dialogueRunner.Add(scriptToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.root.gameObject.name == "Porã")
        {
            if (isAbleToEnter)
            {
                player.loadPointName = exitPoint;
                InstancesManager.singleton.GetLevelLoaderInstance().LoadNextLevel(scene, isLoadingMiniGame ? 1 : 0);
                return;
            }
            else
            {
                NotAbleToEnter();
                return;
            }
        }
    }

    /// <summary>
    /// Checa se o turno do dia é o correto para poder entrar na cena.
    /// </summary>
    /// <returns>
    /// True: Se estiver correto;
    /// False: caso não esteja.
    /// </returns>
    private bool CheckDayShift()
    {
        bool canEnter = false;

        //Usa o singleton para pegar a instância do day manager
        DayManager dayManager = InstancesManager.singleton.GetDayManager();
        int dayShift = dayManager.GetIntDayShift();

        if (dayShiftCondition == DayShift.Nenhum
           || (dayShift == 0 && (dayShiftCondition == DayShift.Manha || dayShiftCondition == DayShift.Manha_Tarde || dayShiftCondition == DayShift.Manha_Noite))
           || (dayShift == 1 && (dayShiftCondition == DayShift.Tarde || dayShiftCondition == DayShift.Manha_Tarde || dayShiftCondition == DayShift.Tarde_Noite))
           || (dayShift == 2 && (dayShiftCondition == DayShift.Noite || dayShiftCondition == DayShift.Manha_Noite || dayShiftCondition == DayShift.Tarde_Noite)))
        {
            canEnter = true;
        }

        return canEnter;
    }

    private bool CheckCantEnterVariables()
    {
        bool canEnter = true;

        //Se não tiver variaveis então pode entrar
        if (arrayCantEnterVariables.Length < 1)
            return canEnter;

        InMemoryVariableStorage inMemoryVariableStorage = InstancesManager.singleton.GetInMemoryVariableStorage();

        foreach (CantEnterVariables cev in arrayCantEnterVariables)
        {
            //Pega a variavel se existir,
            //Se nao existir vai vir null, ou seja tipo diferente, então nao precisa comparar os valores
            Yarn.Value memoryValue = inMemoryVariableStorage.GetValue(cev.name);
            if (memoryValue.type == Yarn.Value.Type.Bool)
            {
                string stringValue = memoryValue.AsBool.ToString();
                if (stringValue.Equals("True"))
                {
                    //Diz que o player não pode entrar
                    canEnter = false;
                    break;
                }
            }
        }

        return canEnter;
    }

    private void NotAbleToEnter()
    {
        dialogueRunner.StartDialogue(notAbleToEnterNode);
    }
}
