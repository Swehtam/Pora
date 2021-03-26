using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ObstaclesManager : MonoBehaviour
{
    //Variaveis para os obstaculos que são pedra
    [SerializeField] private GameObject rocks;
    [SerializeField] private List<Sprite> rocksSprites;
    private readonly float minRocksSpawnIntervalInSeconds = 1.5f; //Tempo minimo para spawnar um obstaculo
    private float maxRocksSpawnIntervalInSeconds = 3f; //Tempo máximo para spawnar um obstaculo
    private int lastRockPosition = 0;
    private bool isRockCouroutineRunning; //Variavel para saber se a Couroutine está sendo executada ou não

    //Variaveis para os obstaculos que são troncos de arvore
    [SerializeField] private GameObject treeTrunks;
    private readonly float minTrunkSpawnIntervalInSeconds = 10f; //Tempo minimo para spawnar um obstaculo
    private float maxTrunkSpawnIntervalInSeconds = 15f; //Tempo máximo para spawnar um obstaculo
    private bool isTrunkCouroutineRunning; //Variavel para saber se a Couroutine está sendo executada ou não

    //Variaveis para os obstaculos que são plantas
    [SerializeField] private List<GameObject> plantsList;
    [SerializeField] private List<Sprite> plantsUpSprites;
    [SerializeField] private List<Sprite> plantsDownSprites;
    private readonly float minPlantsSpawnIntervalInSeconds = 1.5f; //Tempo minimo para spawnar um obstaculo
    private float maxPlantsSpawnIntervalInSeconds = 6f; //Tempo máximo para spawnar um obstaculo
    private bool isPlantsCouroutineRunning; //Variavel para saber se a Couroutine está sendo executada ou não

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
        //Sinalizar que essa Courotine ja esta ocorrendo
        isRockCouroutineRunning = true;

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
        spawned.GetComponent<SpriteRenderer>().sprite = GetRandomRockSpriteFromList();
        spawnedObjects.Add(spawned.GetInstanceID(), spawned);

        yield return new WaitForSeconds(Random.Range(minRocksSpawnIntervalInSeconds, maxRocksSpawnIntervalInSeconds));
        //Depois de Spawnar um obstaculo reduzir seu tempo máximo de Spawn
        if(minRocksSpawnIntervalInSeconds <= maxRocksSpawnIntervalInSeconds)
            maxRocksSpawnIntervalInSeconds -= 0.05f;

        isRockCouroutineRunning = false;

        StartCoroutine(nameof(SpawnRocks));
    }

    private IEnumerator SpawnPlants()
    {

        //Coloca isso para esperar o tempo de transição da camera quando mudar de lado
        //Ou se essa Couroutine ja estver sendo executada, então não execute outra
        //Isso serve para contornar spawnar 2 plantas ao mesmo tempo
        if (dialogueRunner.IsDialogueRunning || isPlantsCouroutineRunning)
        {
            yield break;
        }
        //Sinalizar que essa Courotine ja esta ocorrendo
        isPlantsCouroutineRunning = true;

        //Pega o numero da Zona em que vai ser criado o obstaculo
        //Obs.: Tem q ser negativo, pois o mapa se encontra na posição negativa do eixo Y
        //Obs1.: Tem q substrair 1 para o obstaculo não ser criado em cima da encosta do mapa
        //Obs2.: Esse do...while existe para que crie uma planta em uma zona onde
        int randomSwimZone;
        do
        {
            randomSwimZone = Random.Range(-1, -(player.maxSwimZones + 1)) - 1;
        } while (lastRockPosition == randomSwimZone);

        //Se a zona de spawn for na 2, então a planta será spawnada na primeira zona
        //Nesse caso, a planta só deve empurrar para baixo
        if(randomSwimZone == -2)
        {
            //Planta para empurrar para baixo é a primeira nessa lista
            var spawned = Instantiate(plantsList[0], new Vector2(transform.position.x, randomSwimZone * player.swimZoneHeight), transform.rotation);
            spawned.GetComponent<SpriteRenderer>().sprite = GetRandomPlantDownSpriteFromList();
            spawnedObjects.Add(spawned.GetInstanceID(), spawned);
        }
        //Se a zona de spawn for na 4, então a planta será spawnada na terceira zona
        //Nesse caso, a planta só deve empurrar para cima
        else if(randomSwimZone == -4)
        {
            //Planta para empurrar para cima é a segunda nessa lista
            var spawned = Instantiate(plantsList[1], new Vector2(transform.position.x, randomSwimZone * player.swimZoneHeight), transform.rotation);
            spawned.GetComponent<SpriteRenderer>().sprite = GetRandomPlantUpSpriteFromList();
            spawnedObjects.Add(spawned.GetInstanceID(), spawned);
        }
        //Se a zona for na 3, então a planta será spawnada na segunda zona
        //Nesse caso tanto faz se a planta empurrar para baixo ou para cima
        else
        {
            //O máximo é 2 pois é não incluso
            int upOrDownValue = Random.Range(0, 2);

            //Planta para empurrar para cima é a segunda nessa lista
            var spawned = Instantiate(plantsList[upOrDownValue], new Vector2(transform.position.x, randomSwimZone * player.swimZoneHeight), transform.rotation);
            //Se for 0 então a sprite deve ser da lista de Sprites de Plantas para baixo
            if(upOrDownValue == 0)
            {
                spawned.GetComponent<SpriteRenderer>().sprite = GetRandomPlantDownSpriteFromList();
            }
            //Se não for, então a sprite deve ser da lista de Sprites de Plantas para cima
            else

            {
                spawned.GetComponent<SpriteRenderer>().sprite = GetRandomPlantUpSpriteFromList();
            }
            spawnedObjects.Add(spawned.GetInstanceID(), spawned);
        }

        //Multipla a zona em q vai ser criado o obstaculo com o tamanho das zonas para que o obstaculo se posicione bem no meio do obstaculo
        

        yield return new WaitForSeconds(Random.Range(minPlantsSpawnIntervalInSeconds, maxPlantsSpawnIntervalInSeconds));
        //Depois de Spawnar um obstaculo reduzir seu tempo máximo de Spawn
        if (minRocksSpawnIntervalInSeconds <= maxRocksSpawnIntervalInSeconds)
            maxRocksSpawnIntervalInSeconds -= 0.1f;

        isPlantsCouroutineRunning = false;
        StartCoroutine(nameof(SpawnPlants));
    }

    private IEnumerator SpawnTreeTrunks()
    {
        //Se essa Couroutine ja estver sendo executada, então não execute outra
        //Isso serve para contornar spawnar 2 trunks ao mesmo tempo
        if (isTrunkCouroutineRunning)
            yield break;
        //Sinalizar que essa Courotine ja esta ocorrendo
        isTrunkCouroutineRunning = true;

        //No caso do tronco, eu não quero q ele spawne no começo, então faço ele esperar um pouco antes de spawnar
        yield return new WaitForSeconds(Random.Range(minTrunkSpawnIntervalInSeconds, maxTrunkSpawnIntervalInSeconds));

        //Só é chamado quando o player terminar de executar o dialogo
        //Obs.: Caso a Couroutine estiver esperando enquanto o dialogo ocorra, e o player terminar o dialogo antes da espera acabar, o trunk irá spawnar bem mais cedo (tudo bem!)
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

        isTrunkCouroutineRunning = false;

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
        StartCoroutine(nameof(SpawnPlants));
    }

    public void ResetRocksSpawn()
    {
        maxRocksSpawnIntervalInSeconds = 2f;
    }

    private Sprite GetRandomRockSpriteFromList()
    {
        int randomIndex = Random.Range(0, rocksSprites.Count);
        return rocksSprites[randomIndex];
    }

    private Sprite GetRandomPlantUpSpriteFromList()
    {
        int randomIndex = Random.Range(0, plantsUpSprites.Count);
        return plantsUpSprites[randomIndex];
    }

    private Sprite GetRandomPlantDownSpriteFromList()
    {
        int randomIndex = Random.Range(0, plantsDownSprites.Count);
        return plantsDownSprites[randomIndex];
    }
}