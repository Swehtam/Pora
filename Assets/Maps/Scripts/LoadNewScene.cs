using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LoadNewScene : MonoBehaviour
{
    public string scene;

    public string exitPoint;

    [SerializeField] private bool isLoadingMiniGame = false;
    private PlayerController player;
    private DialogueRunner dialogueRunner;

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
    [Header("Opcional")]
    [Tooltip("Opcional - Turno do dia para poder entrar no minigame.")]
    [SerializeField] private DayShift dayShiftCondition = DayShift.Nenhum;
    [SerializeField] private YarnProgram scriptToLoad;
    [SerializeField] private string notAbleToEnterNode = "";

    void Start()
    {
        //Usa o singleton para pegar a instância do player
        player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();
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
            if (dayShiftCondition == DayShift.Nenhum
               || (DayManager.GetIntDayShift() == 0 && (dayShiftCondition == DayShift.Manha || dayShiftCondition == DayShift.Manha_Tarde || dayShiftCondition == DayShift.Manha_Noite))
               || (DayManager.GetIntDayShift() == 1 && (dayShiftCondition == DayShift.Tarde || dayShiftCondition == DayShift.Manha_Tarde || dayShiftCondition == DayShift.Tarde_Noite))
               || (DayManager.GetIntDayShift() == 2 && (dayShiftCondition == DayShift.Noite || dayShiftCondition == DayShift.Manha_Noite || dayShiftCondition == DayShift.Tarde_Noite)))
            {
                player.loadPointName = exitPoint;
                InstancesManager.singleton.GetLevelLoaderInstance().LoadNextLevel(scene, isLoadingMiniGame ? 1 : 0);
            }
            else
            {
                NotAbleToEnter();
            }
        }
    }

    private void NotAbleToEnter()
    {
        dialogueRunner.StartDialogue(notAbleToEnterNode);
    }
}
