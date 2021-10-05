using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinigamesManager : MonoBehaviour
{
    //Variaveis para Desvio a Nado
    private int distanceMax = 25;
    private int swimDodgeDifficulty = 0;

    void Start()
    {
        //LoadMinigamesVariables();        
    }

    public int GetSwimDodgeMaxDistance()
    {
        return distanceMax;
    }

    public int GetSwimDodgeDifficulty()
    {
        return swimDodgeDifficulty;
    }

    public void ChangeSwimDodgeMaxDistance(int value)
    {
        distanceMax = value;
        InstancesManager.singleton.GetInMemoryVariableStorage().SetValue("$max_distance", value);
    }

    public void UpdateSwimDodgeMaxDistance()
    {
        InstancesManager.singleton.GetInMemoryVariableStorage().SetValue("$max_distance", distanceMax);
    }

    public void ChangeSwimDodgeDifficulty(int value)
    {
        swimDodgeDifficulty = value;
    }

    /// <summary>
    /// Metodo para atualizar todas as variaveis que foram salvas nos arquivos.
    /// </summary>
    public void LoadMinigamesVariables(MinigamesSave minigamesS)
    {
        distanceMax = minigamesS.distanceMax;
        swimDodgeDifficulty = minigamesS.swimDodgeDifficulty;
        InstancesManager.singleton.GetInMemoryVariableStorage().SetValue("$max_distance", distanceMax);
    }

    public MinigamesSave SaveMinigamesVariables()
    {
        MinigamesSave minigamesS = new MinigamesSave
        {
            distanceMax = distanceMax,
            swimDodgeDifficulty = swimDodgeDifficulty
        };
        return minigamesS;
    }
}
