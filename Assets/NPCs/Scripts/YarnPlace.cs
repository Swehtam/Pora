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
    //Fazer lista de string de condi��es que devem ser cumpridas pra acionar OU fazer um administrador desses pontos no mapa para ativar e desativar

    [Header("Opcional")]
    [Tooltip("*Opcional* Dia minimo para o evento do dialogo acontecer.")]
    [SerializeField] private int dayCondition = 0;
    public enum DayShift
    {
        Nenhum = -1,
        /// <summary>
        /// Usando o turno da manh�, significa que a condi��o para o dialogo acontecer s� ser� feito pela se o jogo estiver de manh�.
        /// </summary>
        Manha = 0,
        /// <summary>
        /// Usando o turno da tarde, significa que a condi��o para o dialogo acontecer s� ser� feito pela se o jogo estiver de tarde.
        /// </summary>
        Tarde = 1,
        /// <summary>
        /// Usando o turno da noite, significa que a condi��o para o dialogo acontecer s� ser� feito pela se o jogo estiver de noite.
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

    void Start()
    {
        //Checa se as condi��es necess�rias das variaveis para o YarnPlace est�o concluidas
        //Caso esteja testando e n precise de dialogo
        //E o dia da condi��o pra ativar o dialogo for igual ou maior
        if (isTesting
            || (arrayDialogueVariables.Length > 0 && !CheckAllYarnPlaceVariables())
            || DayManager.GetDay() < dayCondition
            || (int)dayShiftCondition != DayManager.GetIntDayShift())
        {
            gameObject.SetActive(false);
            return;
        }

        if (scriptToLoad != null)
        {
            dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
            dialogueRunner.Add(scriptToLoad);
        }
    }

    //Quando o NPC encontrar o Player mostrar o bot�o para falar
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Se o player entrar na area pra come�ar o dialogo
        if (other.gameObject.CompareTag("Player"))
        {
            dialogueRunner.StartDialogue(talkToNode);
        }
    }

    private bool CheckAllYarnPlaceVariables()
    {
        //Come�a como verdadeira, pois n�o encontrou nenhum problema
        bool hasConditionsCompleted = true;
        //Pega a classe que v� as variaveis na memoria
        var inMemoryVariableStorage = InstancesManager.singleton.GetInMemoryVariableStorage();
        //Verifica se cada uma das variaveis necess�rias para que o YarnPlace seja ativado est� com as condi��es certas
        foreach(Variables va in arrayDialogueVariables)
        {
            //Pega a variavel se existir,
            //Se nao existir vai vir null, ou seja tipo diferente, ent�o nao precisa comparar os valores
            Yarn.Value memoryValue = inMemoryVariableStorage.GetValue(va.name);
            if(va.type == memoryValue.type)
            {
                string stringValue = "";
                switch (va.type)
                {
                    case Yarn.Value.Type.Bool:
                        stringValue = memoryValue.AsBool.ToString();
                        break;
                    case Yarn.Value.Type.Null:
                        stringValue = "null";
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
        //Desativa o local, pois n�o vai precisar mais
        gameObject.SetActive(false);

        //Salvar que esse o dialogo desse local j� aconteceu
        InstancesManager.singleton.GetYarnPlacesManager().YarnPlaceComplete(placeName);
    }
}