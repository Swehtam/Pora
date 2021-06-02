using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinigamesManager : MonoBehaviour
{
    //Variaveis para Desvio a Nado
    private static int distanceMax = 25;
    private static int swimDodgeDifficulty = 0;

    void Start()
    {
        LoadMinigamesVariables();

        ChangeSwimDodgeMaxDistance(distanceMax);
        ChangeSwimDodgeMaxDistance(distanceMax);
    }

    public static int GetSwimDodgeMaxDistance()
    {
        return distanceMax;
    }

    public static int GetSwimDodgeDifficulty()
    {
        return swimDodgeDifficulty;
    }

    public static void ChangeSwimDodgeMaxDistance(int value)
    {
        distanceMax = value;
        InstancesManager.singleton.GetInMemoryVariableStorage().SetValue("$max_distance", value);
    }

    public static void ChangeSwimDodgeDifficulty(int value)
    {
        swimDodgeDifficulty = value;
    }

    /// <summary>
    /// Metodo para atualizar todas as variaveis que foram salvas nos arquivos.
    /// </summary>
    private void LoadMinigamesVariables()
    {

    }
}
