using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ObstaclesManager : MonoBehaviour
{
    //Variaveis para os obstaculos que são pedras
    [SerializeField] private GameObject rocks;
    [SerializeField] private List<Sprite> rocksSprites;
    private readonly float minRocksSpawnIntervalInSeconds = 1.5f; //Tempo minimo para spawnar um obstaculo
    private float maxRocksSpawnIntervalInSeconds = 3f; //Tempo máximo para spawnar um obstaculo
    private int lastRockPosition = 0;
    private bool isRockCouroutineRunning; //Variavel para saber se a Couroutine está sendo executada ou não

    [SerializeField] private GameObject treeTrunks;
    private readonly float minTrunkSpawnIntervalInSeconds = 10f; //Tempo minimo para spawnar um obstaculo
    private float maxTrunkSpawnIntervalInSeconds = 15f; //Tempo máximo para spawnar um obstaculo
    private bool isTrunkCouroutineRunning; //Variavel para saber se a Couroutine está sendo executada ou não

    private PlayerSwimController player;
    public Dictionary<int, GameObject> spawnedObjects = new Dictionary<int, GameObject>();

    private DialogueRunner dialogueRunner;

    private void Awake()
    {
        player = FindObjectOfType<PlayerSwimController>();

        dialogueRunner = FindObjectOfType<DialogueRunner>();
        //Vai chamar o metodo para começar a spawnar obstaculos
        dialogueRunner.onDialogueComplete.AddListener(StartSpawn);
        isRockCouroutineRunning = false;
        isTrunkCouroutineRunning = false;
    }

    private void Update()
    {
        if (DistanceController.isFirstHalfCompleted)
        {
            transform.position = new Vector3(-23f, transform.position.y, transform.position.z);
        }
    }

    private IEnumerator SpawnRocks()
    {

        //Coloca isso para esperar o tempo de transição da camera quando mudar de lado
        //Ou se essa Couroutine ja estver sendo executada, então não execute outra
        //Isso serve para contornar spawnar 2 rocks ao mesmo tempo
        if (dialogueRunner.IsDialogueRunning || isRockCouroutineRunning)
        {
            yield break;
        }

        //Pega o numero da Zona em que vai ser criado o obstaculo
        //Obs.: Tem q ser negativo, pois o mapa se encontra na posição negativa do eixo Y
        //Obs1.: Tem q substrair 1 para o obstaculo não ser criado em cima da encosta do mapa
        //Obs2.: Esse do...while existe para que não se repita a mesma posição da pedra spawnada
        int randomSwimZone;
        do
        {
            randomSwimZone = Random.Range(-1, -(player.maxSwimZones + 1)) - 1;
        } while (lastRockPosition == randomSwimZone);
        //Atualiza qual foi a ultima posição colocada;
        lastRockPosition = randomSwimZone;
        

        //Multipla a zona em q vai ser criado o obstaculo com o tamanho das zonas para que o obstaculo se posicione bem no meio do obstaculo
        var spawned = Instantiate(rocks, new Vector2(transform.position.x, randomSwimZone * player.swimZoneHeight), transform.rotation);
        spawned.GetComponent<SpriteRenderer>().sprite = GetRandomSpriteFromList();
        spawnedObjects.Add(spawned.GetInstanceID(), spawned);

        yield return new WaitForSeconds(Random.Range(minRocksSpawnIntervalInSeconds, maxRocksSpawnIntervalInSeconds));
        //Depois de Spawnar um obstaculo reduzir seu tempo máximo de Spawn
        if(minRocksSpawnIntervalInSeconds <= maxRocksSpawnIntervalInSeconds)
            maxRocksSpawnIntervalInSeconds -= 0.05f;
            
        StartCoroutine(nameof(SpawnRocks));
    }

    //Só é chamado quando o player terminar de executar o dialogo
    //Obs.: Caso a Couroutine estiver esperando enquanto o dialogo ocorra, e o player terminar o dialogo antes da espera acabar, o trunk irá spawnar bem mais cedo (tudo bem!)
    private IEnumerator SpawnTreeTrunks()
    {
        //Se essa Couroutine ja estver sendo executada, então não execute outra
        //Isso serve para contornar spawnar 2 trunks ao mesmo tempo
        if (isTrunkCouroutineRunning)
            yield break;

        //No caso do tronco, eu não quero q ele spawne no começo, então faço ele esperar um pouco antes de spawnar
        yield return new WaitForSeconds(Random.Range(minTrunkSpawnIntervalInSeconds, maxTrunkSpawnIntervalInSeconds));

        //Coloca isso para esperar o tempo de transição da camera quando mudar de lado
        if (dialogueRunner.IsDialogueRunning)
        {
            yield break;
        }

        //Multipla a zona em q vai ser criado o obstaculo com o tamanho das zonas para que o obstaculo se posicione bem no meio do obstaculo
        var spawned = Instantiate(treeTrunks, new Vector2(transform.position.x, -6f), transform.rotation);
        spawnedObjects.Add(spawned.GetInstanceID(), spawned);
        //Depois de Spawnar um obstaculo reduzir seu tempo máximo de Spawn
        if (minTrunkSpawnIntervalInSeconds <= maxTrunkSpawnIntervalInSeconds)
            maxTrunkSpawnIntervalInSeconds -= 1f;

        StartCoroutine(nameof(SpawnTreeTrunks));
    }

    public void DestroyObstacle(GameObject obstacle)
    {
        if (spawnedObjects.ContainsKey(obstacle.GetInstanceID()))
        {
            spawnedObjects.Remove(obstacle.GetInstanceID());
            Destroy(obstacle);
        }
    }

    public void DestroyAllSpawnedObjects()
    {
        //Destroi todos os objetos que estão nesse dicionario
        foreach (GameObject obstacle in spawnedObjects.Values)
        {
            Destroy(obstacle);
        }

        //Limpa o dicionário
        spawnedObjects.Clear();
    }

    //Metodo chamado toda vez q o dialogo terminar de ser executado
    private void StartSpawn()
    {
        StartCoroutine(nameof(SpawnRocks));
        StartCoroutine(nameof(SpawnTreeTrunks));
    }

    public void ResetRocksSpawn()
    {
        maxRocksSpawnIntervalInSeconds = 2f;
    }

    private Sprite GetRandomSpriteFromList()
    {
        int randomIndex = Random.Range(0, rocksSprites.Count);
        return rocksSprites[randomIndex];
    }
}
