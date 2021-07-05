using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceController : MonoBehaviour
{
    [SerializeField] private TMP_Text display;
    [SerializeField] private SwimDodgeClassesInterface swimDodgeClassesInterface;
    private float distanceSwimmed = 0f;
    private bool finished = false;

    //Booleano para saber se o player está indo ou voltando
    public static bool isFirstHalfCompleted = false;

    private void Start()
    {
        isFirstHalfCompleted = false;
    }

    private void FixedUpdate()
    {
        //Distância é igual a velocidade * tempo decorrido
        //Nesse caso o tempo é o Time.fixedDeltaTime é o tempo exato em q cada frame desse FixedUpdate é chamado
        //Dividir por 4, pois nesse jogo 4 unidades de distancia vai corresponder a 1 metro percorrido
        //Multiplicado por -1 pois a velocidade é negativa
        distanceSwimmed += -(SpeedController.speed * Time.fixedDeltaTime)/4;

        display.text = Mathf.FloorToInt(distanceSwimmed).ToString() + "m";

        //Controla qual a meta que Porã deve chegar
        if(distanceSwimmed >= MinigamesManager.GetSwimDodgeMaxDistance() && !isFirstHalfCompleted)
        {
            isFirstHalfCompleted = true;
            SpeedController.speed = 0f;
            swimDodgeClassesInterface.minigameDialogue.StartSecondDialogue();
            swimDodgeClassesInterface.obstaclesManager.ResetRocksSpawn();
            swimDodgeClassesInterface.obstaclesManager.DisableAllSpawnedObjects();
            swimDodgeClassesInterface.buttonsPositionController.ChangeButtonsSide();
        }

        if(distanceSwimmed <= 0 && isFirstHalfCompleted && !finished)
        {
            finished = true;
            SpeedController.speed = 0f;
            //Acionar todos os eventos que dependem do player terminar o minigame de Nado a Desvio
            QuestEvents questEvents = InstancesManager.singleton.GetQuestEvents();
            questEvents.SwimDodgeCompleted(MinigamesManager.GetSwimDodgeDifficulty(), MinigamesManager.GetSwimDodgeMaxDistance());
            swimDodgeClassesInterface.minigameDialogue.StartFinishingDialogue();
            swimDodgeClassesInterface.obstaclesManager.DisableAllSpawnedObjects();
        }

        if (finished) SpeedController.speed = 0f;
    }

    public void ResetGame()
    {
        isFirstHalfCompleted = false;
    }
}