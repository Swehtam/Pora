using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnableObjects;
    [SerializeField] private List<Sprite> obstaclesSprites;

    private PlayerSwimController player;
    public Dictionary<int, GameObject> spawnedObjects = new Dictionary<int, GameObject>();
    private void Awake()
    {
        player = FindObjectOfType<PlayerSwimController>();
        //Subscribes to Reset of Player
        player.OnReset += DestroyAllSpawnedObjects;

        StartCoroutine(nameof(Spawn));
    }

    private IEnumerator Spawn()
    {   //Pega o numero da Zona em que vai ser criado o obstaculo
        //Obs.: Tem q ser negativo, pois o mapa se encontra na posição negativa do eixo Y
        //Obs1.: Tem q substrair 1 para o obstaculo não ser criado em cima da encosta do mapa
        int randomSwimZone = Random.Range(-1, -(player.maxSwimZones+1)) - 1;

        //Multipla a zona em q vai ser criado o obstaculo com o tamanho das zonas para que o obstaculo se posicione bem no meio do obstaculo
        var spawned = Instantiate(GetRandomSpawnableFromList(), new Vector2(transform.position.x, randomSwimZone * player.swimZoneHeight), transform.rotation);
        spawned.GetComponent<SpriteRenderer>().sprite = GetRandomSpriteFromList();
        spawnedObjects.Add(spawned.GetInstanceID(), spawned);

        yield return new WaitForSeconds(Random.Range(SpeedController.minSpawnIntervalInSeconds, SpeedController.maxSpawnIntervalInSeconds));
        StartCoroutine(nameof(Spawn));
    }

    public void DestroyObstacle(GameObject obstacle)
    {
        if (spawnedObjects.ContainsKey(obstacle.GetInstanceID()))
        {
            spawnedObjects.Remove(obstacle.GetInstanceID());
            Destroy(obstacle);
        }
    }

    private void DestroyAllSpawnedObjects()
    {
        foreach(int key in spawnedObjects.Keys)
        {
            Destroy(spawnedObjects[key]);
            spawnedObjects.Remove(key);
        }

        Debug.Log("quantidade de objetos no dicionário é de: " + spawnedObjects.Count);
    }
    private GameObject GetRandomSpawnableFromList()
    {
        int randomIndex = Random.Range(0, spawnableObjects.Count);
        return spawnableObjects[randomIndex];
    }

    private Sprite GetRandomSpriteFromList()
    {
        int randomIndex = Random.Range(0, obstaclesSprites.Count);
        return obstaclesSprites[randomIndex];
    }
}
