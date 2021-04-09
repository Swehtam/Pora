using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private CinemachineVirtualCamera vcam;

    //Tem que buscar o player no Start, pois ele ainda n foi criado no Awake
    //Instancia do Player está sendo criado no Awake do GameController
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (player is null) player = InstancesManager.singleton.GetPlayerInstance();

        vcam.m_Follow = player.transform;
    }
}
