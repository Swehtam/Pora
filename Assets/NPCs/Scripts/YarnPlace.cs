using UnityEngine;
using Yarn.Unity;
using System.Collections.Generic;
using System;

public class YarnPlace : MonoBehaviour
{
    public bool isTesting = false;
    public string placeName = "";
    public string talkToNode = "";
    public YarnProgram scriptToLoad;
    //Fazer lista de string de condições que devem ser cumpridas pra acionar OU fazer um administrador desses pontos no mapa para ativar e desativar

    [Header("Opcional")]
    [Tooltip("*Opcional* Dia minimo para o evento do dialogo acontecer.")]
    [SerializeField] private int dayCondition = 0;
    public enum DayShift
    {
        Nenhum = -1,
        /// <summary>
        /// Usando o turno da manhã, significa que a condição para o dialogo acontecer só será feito pela se o jogo estiver de manhã.
        /// </summary>
        Manha = 0,
        /// <summary>
        /// Usando o turno da tarde, significa que a condição para o dialogo acontecer só será feito pela se o jogo estiver de tarde.
        /// </summary>
        Tarde = 1,
        /// <summary>
        /// Usando o turno da noite, significa que a condição para o dialogo acontecer só será feito pela se o jogo estiver de noite.
        /// </summary>
        Noite = 2
    }
    [Tooltip("Turno do dia, opcional, para o evento do dialogo acontecer.")]
    [SerializeField] private DayShift dayShiftCondition = DayShift.Nenhum;
    [Serializable]
    public struct Variables
    {
        /// <summary>
        /// The name of the variable.
        /// </summary>
        /// <remarks>
        /// Do not include the `$` prefix in front of the variable
        /// name. It will be added for you.
        /// </remarks>
        public string name;

        /// <summary>
        /// The value of the variable, as a string.
        /// </summary>
        /// <remarks>
        /// This string will be converted to the appropriate type,
        /// depending on the value of <see cref="type"/>.
        /// </remarks>
        public string value;

        /// <summary>
        /// The type of the variable.
        /// </summary>
        public Yarn.Value.Type type;
    }
    public Variables[] arrayDialogueVariables;
    private DialogueRunner dialogueRunner = null;
    private QuestEvents questEvents;

    void Start()
    {
        var dayManager = InstancesManager.singleton.GetDayManager();
        //Desativa esse objeto se:
        //Tiver variaveis e alguma delas não estiver correta
        //Ou caso esteja testando
        //Ou não chegou no dia minimo e está no horário do dia errado
        if (isTesting
            || (arrayDialogueVariables.Length > 0 && CheckAllYarnPlaceVariables() == false)
            || (dayManager.GetDay() < dayCondition) 
            || ((dayShiftCondition != DayShift.Nenhum) && ((int)dayShiftCondition != dayManager.GetIntDayShift()))
            || InstancesManager.singleton.GetYarnPlacesManager().ContainsYarnPlace(placeName))
        {
            gameObject.SetActive(false);
            return;
        }

        questEvents = InstancesManager.singleton.GetQuestEvents();

        if (scriptToLoad != null)
        {
            dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
            dialogueRunner.Add(scriptToLoad);
        }
    }

    //Quando o NPC encontrar o Player mostrar o botão para falar
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Se o player entrar na area pra começar o dialogo
        if (other.gameObject.CompareTag("Player"))
        {
            //Acionar todos os eventos que dependem do player chegar em determinado lugar
            questEvents.PlaceArrive(placeName);
            //Iniciar dialogo
            dialogueRunner.StartDialogue(talkToNode);
        }
    }

    /// <summary>
    /// Checa todas as variaveis listadas presentes no objeto
    /// </summary>
    /// <returns>
    /// Se todas as variaveis estiverem corretas então retorna TRUE
    /// Se não retorna FALSE
    /// </returns>
    private bool CheckAllYarnPlaceVariables()
    {
        //Começa como verdadeira, pois não encontrou nenhum problema
        bool hasConditionsCompleted = true;
        //Pega a classe que vê as variaveis na memoria
        var inMemoryVariableStorage = InstancesManager.singleton.GetInMemoryVariableStorage();
        //Verifica se cada uma das variaveis necessárias para que o YarnPlace seja ativado está com as condições certas
        foreach(Variables va in arrayDialogueVariables)
        {
            //Pega a variavel se existir,
            //Se nao existir vai vir null, ou seja tipo diferente, então nao precisa comparar os valores
            Yarn.Value memoryValue = inMemoryVariableStorage.GetValue(va.name);
            if(va.type == memoryValue.type)
            {
                string stringValue = "";
                switch (va.type)
                {
                    case Yarn.Value.Type.Bool:
                        stringValue = memoryValue.AsBool.ToString();
                        break;
                    case Yarn.Value.Type.Number:
                        stringValue = memoryValue.AsNumber.ToString();
                        break;
                    case Yarn.Value.Type.String:
                        stringValue = memoryValue.AsString;
                        break;
                    default:
                        stringValue = "<unknown>";
                        break;
                }

                if(!stringValue.Equals(va.value))
                {
                    hasConditionsCompleted = false;
                    break;
                }
            }
            else
            {
                hasConditionsCompleted = false;
                break;
            }
        }
        return hasConditionsCompleted;
    }

    [YarnCommand("placeDone")]
    public void SetYarnPlaceAsDone()
    {
        //Desativa o local, pois não vai precisar mais
        gameObject.SetActive(false);

        //Salvar que esse o dialogo desse local já aconteceu
        InstancesManager.singleton.GetYarnPlacesManager().YarnPlaceComplete(placeName);
    }
}
