using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class SwimDodgeMods : MonoBehaviour
{
    [Serializable]
    public class DistanceToggles
    {
        /// <summary>
        /// O nome do Toggle
        /// </summary>
        public string name;

        /// <summary>
        /// O valor do toggle
        /// </summary>
        public int value;

        /// <summary>
        /// O toggle referenciado
        /// </summary>
        public Toggle toggle;
    }

    /// <summary>
    /// A lista dos toogles usados no Painel de opções do
    /// minigame Desvio a Nado
    /// </summary>
    public DistanceToggles[] distanceTogglesVector;

    /// Onde realmente mantem os toggles
    private readonly Dictionary<int, Toggle> distanceToggle = new Dictionary<int, Toggle>();
    private int lastDistanceValue;
    private MinigamesManager minigamesManager;

    private void Start()
    {
        minigamesManager = InstancesManager.singleton.GetMinigamesManager();

        foreach(DistanceToggles distanceTG in distanceTogglesVector)
        {
            distanceToggle.Add(distanceTG.value, distanceTG.toggle);
        }

        //Atualiza o Toggle para o ultimo que foi salvo e atualiza nesse painel;
        lastDistanceValue = minigamesManager.GetSwimDodgeMaxDistance();
        distanceToggle[minigamesManager.GetSwimDodgeMaxDistance()].isOn = true;
    }
    /// <summary>
    /// Metodo para ser chamado pelos botões de dificuldade.
    /// Então alterar o variavel responsavel pela dificuldade do minigame no MinigamesManager.
    /// </summary>
    /// <param name="difficulty">
    /// 0: Fácil;
    /// 1: Médio;
    /// 2: Dificil;</param>
    public void ChangeDifficulty(int difficulty)
    {
        minigamesManager.ChangeSwimDodgeDifficulty(difficulty);
    }

    public void ChangeMaxDistance(int value)
    {
        if(value != lastDistanceValue)
        {
            minigamesManager.ChangeSwimDodgeMaxDistance(value);
            lastDistanceValue = value;
        }
    }
}
