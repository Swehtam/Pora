using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : Joystick
{
    private GameObject player;

    protected override void Start()
    {
        base.Start();
        if (player is null) player = InstancesManager.singleton.getPlayerInstance();
        player.GetComponent<PlayerController>().SetJoystick(this.gameObject.GetComponent<FixedJoystick>());
    }
}