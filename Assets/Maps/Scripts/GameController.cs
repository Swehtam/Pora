using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        GameObject player = InstancesManager.singleton.getPlayerInstance();
        if(player == null)
        {
            player = Instantiate(Resources.Load("Prefabs/Porã")) as GameObject;
            //Mudar o script que será utilizado
            PlayerSettings playerSettings = InstancesManager.singleton.getPlayerSettingsInstance();
            player.GetComponent<PlayerController>().loadPointName = playerSettings.playerStartPoint;
            //Tem q setar como 'Porã' pq quando instancia é criado com o nome (Clone) no final
            player.transform.name = "Porã";
        }
    }
}