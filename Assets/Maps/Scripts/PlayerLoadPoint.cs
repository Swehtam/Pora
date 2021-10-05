using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadPoint : MonoBehaviour
{
    public string pointName;
    public bool isTesting;

    // Start is called before the first frame update
    void Start()
    {
        if (isTesting)
            return;

        PlayerController player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();

        if(player != null && player.loadPointName == pointName)
        {
            player.transform.position = transform.position;
        }
    }
}
