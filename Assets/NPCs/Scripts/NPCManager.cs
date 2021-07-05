using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Yarn.Unity;

public class NPCManager : MonoBehaviour
{
    [Serializable]
    public struct NPCS
    {
        /// <summary>
        /// O nome do NPC
        /// </summary>
        public string name;

        /// <summary>
        /// O seu GameObject
        /// </summary>
        public GameObject gameObject;
    }
    [Tooltip("Lista de NPCs que não são default na cena, mas que vão aparecer")]
    public NPCS[] npcs;

    public Transform[] poraScenePositions;

    private Dictionary<string, GameObject> npcDict = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        if (npcs.Length > 0)
            LoadNPCDictionary();
    }

    /// <summary>
    /// Metodo chamado pelo PlayerDialogueCommand para pegar a posição para onde o player vai andar
    /// </summary>
    /// <param name="positionID"></param>
    /// <returns></returns>
    public Vector3 GetPoraScenePosition(int positionID)
    {
        return poraScenePositions[positionID].position;
    }

    private void LoadNPCDictionary()
    {
        foreach(NPCS npc in npcs)
        {
            npcDict.Add(npc.name, npc.gameObject);
        }
    }

    [YarnCommand("showNPC")]
    public void ShowNPC(string npcName, string stringPositionID)
    {
        if (int.TryParse(stringPositionID, out var positionID) == false)
        {
            Debug.LogErrorFormat($"<<showNPC>> failed to parse position id {stringPositionID}");
            return;
        }

        //Pega o GameObject do NPC
        GameObject npcGameObject = npcDict[npcName];
        //Seleciona a posição que ele vai spawnar
        npcGameObject.GetComponent<NPCSceneSpawner>().SpawnNPC(positionID);
        //Ativa o NPC
        npcGameObject.SetActive(true);        
    }

    [YarnCommand("teleportPlayer")]
    public void TeleportPlayer(string stringPositionID)
    {
        if (int.TryParse(stringPositionID, out var positionID) == false)
        {
            Debug.LogErrorFormat($"<<teleportPlayer>> failed to parse position id {stringPositionID}");
            return;
        }

        PlayerController player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();
        Transform placeTransform = poraScenePositions[positionID];
        player.TeleportTo(placeTransform);
    }
}
