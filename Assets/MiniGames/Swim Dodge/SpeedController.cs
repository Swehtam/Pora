using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    //Static para compartilhar a mesma velocidade com todos os objetos do Mini Game
    public static float speed = -2f;
    [Tooltip("The Spawner waits a random number of seconds between these two interval each time a object was spawned.")]
    public static float minSpawnIntervalInSeconds = 2f;
    public static float maxSpawnIntervalInSeconds = 4f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(speed >= -4f)
        {
            speed += Time.deltaTime * -0.1f;
        }
        
        if(maxSpawnIntervalInSeconds >= minSpawnIntervalInSeconds)
        {
            maxSpawnIntervalInSeconds -= Time.deltaTime * 0.05f;
        }
    }
}
