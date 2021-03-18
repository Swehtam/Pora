using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private CinemachineVirtualCamera vcam;

    // Start is called before the first frame update
    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (player is null) player = InstancesManager.singleton.getPlayerInstance();

        vcam.Follow = player.transform;
    }
}
