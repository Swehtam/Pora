﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadPoint : MonoBehaviour
{
    private PlayerController player;

    public string pointName;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        if(player.loadPointName == pointName)
        {
            player.transform.position = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
