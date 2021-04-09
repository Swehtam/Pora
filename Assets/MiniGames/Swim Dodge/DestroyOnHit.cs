using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    [SerializeField] private MinigameClassesInterface minigameClassesInterface;
    private void Update()
    {
        if (DistanceController.isFirstHalfCompleted)
        {
            transform.position = new Vector3(0f, transform.position.y, transform.position.z);  
        }
    }

    //Script para destruir todos os obstaculos
    //Teoricamente não vai destruir o background, pois no start o seu collider é desativado
    private void OnTriggerEnter2D(Collider2D other)
    {
        minigameClassesInterface.obstaclesManager.DisableObstacle(other.gameObject, other.gameObject.GetComponent<Obstacle>().id);
    }
}
