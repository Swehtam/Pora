using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    private GameObject player;
    private Vector3 backgroundInitialPosition;

    protected override void Start()
    {
        base.Start();
        backgroundInitialPosition = background.transform.position;

        if (player is null) player = InstancesManager.singleton.GetPlayerInstance();
        player.GetComponent<PlayerController>().SetJoystick(this.gameObject.GetComponent<FloatingJoystick>());
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //Reseta o background
        background.transform.position = backgroundInitialPosition;

        base.OnPointerUp(eventData);
    }

    public override void ResetJoystick()
    {
        base.ResetJoystick();
        background.transform.position = backgroundInitialPosition;
    }
}