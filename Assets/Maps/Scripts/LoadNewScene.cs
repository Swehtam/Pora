using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNewScene : MonoBehaviour
{
    public string scene;

    public string exitPoint;

    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        //Usa o singleton para pegar a instância do player
        player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.root.gameObject.name == "Porã" && !collision.isTrigger)
        {
            player.loadPointName = exitPoint;
            InstancesManager.singleton.GetLevelLoaderInstance().LoadNextLevel(scene);
        }
    }
}
