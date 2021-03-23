using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceController : MonoBehaviour
{
    [SerializeField] private TMP_Text display;
    private float distanceSwimmed = 0f;
    //Booleano para saber se o player está indo ou voltando
    public static bool isFirstHalfCompleted = false;

    public MinigameDialogue minigameDialogue;
    public ObstaclesManager obstaclesManager;

    private void FixedUpdate()
    {
        //Distância é igual a velocidade * tempo decorrido
        //Nesse caso o tempo é o Time.fixedDeltaTime é o tempo exato em q cada frame desse FixedUpdate é chamado
        //Dividir por 4, pois nesse jogo 4 unidades de distancia vai corresponder a 1 metro percorrido
        //Multiplicado por -1 pois a velocidade é negativa
        distanceSwimmed += -(SpeedController.speed * Time.fixedDeltaTime)/4;

        display.text = Mathf.FloorToInt(distanceSwimmed).ToString() + "m";

        //Controla qual a meta que Porã deve chegar
        if(distanceSwimmed >= 5f && !isFirstHalfCompleted)
        {
            isFirstHalfCompleted = true;
            SpeedController.speed = 0f;
            minigameDialogue.StartSecondDialogue();
            obstaclesManager.ResetRocksSpawn();
            obstaclesManager.DestroyAllSpawnedObjects();
        }
    }
}