using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ObstaclesManager : MonoBehaviour
{
    //Variaveis para os obstaculos que são pedra
    [SerializeField] private GameObject rocks;
    [SerializeField] private List<Sprite> rocksSprites;
    [SerializeField] private List<GameObject> disabledRocks = new List<GameObject>();
    private readonly float minRocksSpawnIntervalInSeconds = 1.5f; //Tempo minimo para spawnar um obstaculo
    private float maxRocksSpawnIntervalInSeconds = 3f; //Tempo máximo para spawnar um obstaculo
    private int lastRockPosition = 0;
    private bool isRockCouroutineRunning; //Variavel para saber se a Couroutine está sendo executada ou não

    //Variaveis para os obstaculos que são troncos de arvore
    [SerializeField] private GameObject treeTrunks;
    [SerializeField] private List<GameObject> disabledTreeTrunks = new List<GameObject>();
    private readonly float minTrunkSpawnIntervalInSeconds = 10f; //Tempo minimo para spawnar um obstaculo
    private float maxTrunkSpawnIntervalInSeconds = 15f; //Tempo máximo para spawnar um obstaculo
    private bool isTrunkCouroutineRunning; //Variavel para saber se a Couroutine está sendo executada ou não

    //Variaveis para os obstaculos que são plantas
    [SerializeField] private List<GameObject> plantsList;
    [SerializeField] private List<Sprite> plantsUpSprites;
    [SerializeField] private List<Sprite> plantsDownSprites;
    [SerializeField] private List<GameObject> disabledPlantsUp = new List<GameObject>();
    [SerializeField] private List<GameObject> disabledPlantsDown = new List<GameObject>();
    private readonly float minPlantsSpawnIntervalInSeconds = 4f; //Tempo minimo para spawnar um obstaculo
    private float maxPlantsSpawnIntervalInSeconds = 6f; //Tempo máximo para spawnar um obstaculo
    private bool isPlantsCouroutineRunning; //Variavel para saber se a Couroutine está sendo executada ou não

    [SerializeField] private SwimDodgeClassesInterface swimDodgeClassesInterface;
    public Dictionary<int, GameObject> spawnedObjects = new Dictionary<int, GameObject>();

    private DialogueRunner dialogueRunner;

    private void Awake()
    {
        dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
        //Vai chamar o metodo para começar a spawnar obstaculos
        dialogueRunner.onDialogueComplete.AddListener(StartSpawn);
        isRockCouroutineRunning = false;
        isTrunkCouroutineRunning = false;
        isPlantsCouroutineRunning = false;

        //Adiciona no Spawned Obejects os objects que já estão presentes na cena
        foreach (GameObject rock in disabledRocks)
        {
            spawnedObjects.Add(rock.GetInstanceID(), rock);
        }

        foreach (GameObject treeTrunk in disabledTreeTrunks)
        {
            spawnedObjects.Add(treeTrunk.GetInstanceID(), treeTrunk);
        }

        foreach (GameObject plant in disabledPlantsUp)
        {
            spawnedObjects.Add(plant.GetInstanceID(), plant);
        }

        foreach (GameObject plant in disabledPlantsDown)
        {
            spawnedObjects.Add(plant.GetInstanceID(), plant);
        }
    }

    private void Update()
    {
        if (DistanceController.isFirstHalfCompleted)
        {
            transform.position = new Vector3(-24.5f, transform.position.y, transform.position.z);
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
            randomSwimZone = Random.Range(-1, -(swimDodgeClassesInterface.player.maxSwimZones + 1)) - 1;
        } while (lastRockPosition == randomSwimZone);
        //Atualiza qual foi a ultima posição colocada;
        lastRockPosition = randomSwimZone;

        //Se tiver pedras desativadas, então reutilize
        if(disabledRocks.Count > 0)
        {
            var spawned = GetRandomDisabledRock();
            spawned.transform.position = new Vector2(transform.position.x, randomSwimZone * swimDodgeClassesInterface.player.swimZoneHeight);
            spawned.GetComponent<SpriteRenderer>().sprite = GetRandomRockSpriteFromList();
            spawned.SetActive(true);
        }
        //Se não crie nova pedra
        else
        {
            //Multipla a zona em q vai ser criado o obstaculo com o tamanho das zonas para que o obstaculo se posicione bem no meio do obstaculo
            var spawned = Instantiate(rocks, new Vector2(transform.position.x, randomSwimZone * swimDodgeClassesInterface.player.swimZoneHeight), transform.rotation);
            spawned.GetComponent<SpriteRenderer>().sprite = GetRandomRockSpriteFromList();
            spawnedObjects.Add(spawned.GetInstanceID(), spawned);
        }
        

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
            randomSwimZone = Random.Range(-1, -(swimDodgeClassesInterface.player.maxSwimZones + 1)) - 1;
        } while (lastRockPosition == randomSwimZone);

        //Se a zona de spawn for na 2, então a planta será spawnada na primeira zona
        //Nesse caso, a planta só deve empurrar para baixo
        if(randomSwimZone == -2)
        {
            //Se tiver plantas que empurram para baixo desativas, então reutilize
            if (disabledPlantsDown.Count > 0)
            {
                var spawned = GetRandomDisabledPlantsDown();
                spawned.transform.position = new Vector2(transform.position.x, randomSwimZone * swimDodgeClassesInterface.player.swimZoneHeight);
                spawned.GetComponent<SpriteRenderer>().sprite = GetRandomPlantDownSpriteFromList();
                spawned.SetActive(true);
            }
            //Se não crie uma nova
            else
            {
                //Planta para empurrar para baixo é a primeira nessa lista
                var spawned = Instantiate(plantsList[0], new Vector2(transform.position.x, randomSwimZone * swimDodgeClassesInterface.player.swimZoneHeight), transform.rotation);
                spawned.GetComponent<SpriteRenderer>().sprite = GetRandomPlantDownSpriteFromList();
                spawnedObjects.Add(spawned.GetInstanceID(), spawned);
            }
            
        }
        //Se a zona de spawn for na 4, então a planta será spawnada na terceira zona
        //Nesse caso, a planta só deve empurrar para cima
        else if(randomSwimZone == -4)
        {
            //Se tiver plantas que empurram para cima desativas, então reutilize
            if (disabledPlantsUp.Count > 0)
            {
                var spawned = GetRandomDisabledPlantsUp();
                spawned.transform.position = new Vector2(transform.position.x, randomSwimZone * swimDodgeClassesInterface.player.swimZoneHeight);
                spawned.GetComponent<SpriteRenderer>().sprite = GetRandomPlantUpSpriteFromList();
                spawned.SetActive(true);
            }
            else
            {
                //Planta para empurrar para cima é a segunda nessa lista
                var spawned = Instantiate(plantsList[1], new Vector2(transform.position.x, randomSwimZone * swimDodgeClassesInterface.player.swimZoneHeight), transform.rotation);
                spawned.GetComponent<SpriteRenderer>().sprite = GetRandomPlantUpSpriteFromList();
                spawnedObjects.Add(spawned.GetInstanceID(), spawned);
            }
        }
        //Se a zona for na 3, então a planta será spawnada na segunda zona
        //Nesse caso tanto faz se a planta empurrar para baixo ou para cima
        else
        {
            //O máximo é 2 pois é não incluso
            int upOrDownValue = Random.Range(0, 2);

            //Então é uma planta que empurra para baixo
            if(upOrDownValue == 0)
            {
                //Se tiver plantas que empurram para baixo desativas, então reutilize
                if (disabledPlantsDown.Count > 0)
                {
                    var spawned = GetRandomDisabledPlantsDown();
                    spawned.transform.position = new Vector2(transform.position.x, randomSwimZone * swimDodgeClassesInterface.player.swimZoneHeight);
                    spawned.GetComponent<SpriteRenderer>().sprite = GetRandomPlantDownSpriteFromList();
                    spawned.SetActive(true);
                }
                //Se não crie uma nova
                else
                {
                    //Planta para empurrar para baixo é a primeira nessa lista
                    var spawned = Instantiate(plantsList[0], new Vector2(transform.position.x, randomSwimZone * swimDodgeClassesInterface.player.swimZoneHeight), transform.rotation);
                    spawned.GetComponent<SpriteRenderer>().sprite = GetRandomPlantDownSpriteFromList();
                    spawnedObjects.Add(spawned.GetInstanceID(), spawned);
                }
            }
            else
            {
                //Se tiver plantas que empurram para cima desativas, então reutilize
                if (disabledPlantsUp.Count > 0)
                {
                    var spawned = GetRandomDisabledPlantsUp();
                    spawned.transform.position = new Vector2(transform.position.x, randomSwimZone * swimDodgeClassesInterface.player.swimZoneHeight);
                    spawned.GetComponent<SpriteRenderer>().sprite = GetRandomPlantUpSpriteFromList();
                    spawned.SetActive(true);
                }
                else
                {
                    //Planta para empurrar para cima é a segunda nessa lista
                    var spawned = Instantiate(plantsList[1], new Vector2(transform.position.x, randomSwimZone * swimDodgeClassesInterface.player.swimZoneHeight), transform.rotation);
                    spawned.GetComponent<SpriteRenderer>().sprite = GetRandomPlantUpSpriteFromList();
                    spawnedObjects.Add(spawned.GetInstanceID(), spawned);
                }
            }
        }

        yield return new WaitForSeconds(Random.Range(minPlantsSpawnIntervalInSeconds, maxPlantsSpawnIntervalInSeconds));
        //Depois de Spawnar um obstaculo reduzir seu tempo máximo de Spawn
        if (minPlantsSpawnIntervalInSeconds <= maxPlantsSpawnIntervalInSeconds)
            maxPlantsSpawnIntervalInSeconds -= 0.1f;

        isPlantsCouroutineRunning = false;
        StartCoroutine(nameof(SpawnPlants));
    }

    private IEnumerator SpawnTreeTrunks()
    {
        //Se essa Couroutine ja estver sendo executada, então não execute outra
        //Isso serve para contornar spawnar 2 trunks ao mesmo tempo
        if (isTrunkCouroutineRunning || dialogueRunner.IsDialogueRunning)
            yield break;
        //Sinalizar que essa Courotine ja esta ocorrendo
        isTrunkCouroutineRunning = true;

        //Se tiver tronos de arvores desativadas, então reutilize
        if (disabledTreeTrunks.Count > 0)
        {
            var spawned = GetRandomDisabledTreeTrunk();
            spawned.transform.position = new Vector2(transform.position.x, -6f);
            spawned.SetActive(true);
        }
        //Se não crie um novo tronco
        else
        {
            //Multipla a zona em q vai ser criado o obstaculo com o tamanho das zonas para que o obstaculo se posicione bem no meio do obstaculo
            var spawned = Instantiate(treeTrunks, new Vector2(transform.position.x, -6f), transform.rotation);
            spawnedObjects.Add(spawned.GetInstanceID(), spawned);
        }

        yield return new WaitForSeconds(Random.Range(minTrunkSpawnIntervalInSeconds, maxTrunkSpawnIntervalInSeconds));

        //Depois de Spawnar um obstaculo reduzir seu tempo máximo de Spawn
        if (minTrunkSpawnIntervalInSeconds <= maxTrunkSpawnIntervalInSeconds)
            maxTrunkSpawnIntervalInSeconds -= 1f;

        isTrunkCouroutineRunning = false;

        StartCoroutine(nameof(SpawnTreeTrunks));
    }

    public void DisableObstacle(GameObject obstacle, int id)
    {
        //Checa se o obstaculo existe no dicionário de objetos
        if (spawnedObjects.ContainsKey(obstacle.GetInstanceID()))
        {
            switch (id)
            {
                //Se for uma pedra
                case 0:
                    disabledRocks.Add(obstacle);
                    break;
                //Se for um tronco de arvore
                case 1:
                    disabledTreeTrunks.Add(obstacle);
                    break;
                //Se for uma planta que empurra para baixo
                case 2:
                    disabledPlantsDown.Add(obstacle);
                    break;
                //Se for uma planta que empurra para cima
                case 3:
                    disabledPlantsUp.Add(obstacle);
                    break;
            }
            //Desativa o obstaculo
            obstacle.SetActive(false);
        }
    }

    //Metodo chamado quando Porã for nadar para o outro lado
    public void DisableAllSpawnedObjects()
    {
        //Desativa as Coroutines
        //E reseta seus sinalizadores
        StopCoroutine(nameof(SpawnRocks));
        isRockCouroutineRunning = false;
        StopCoroutine(nameof(SpawnPlants));
        isPlantsCouroutineRunning = false;
        StopCoroutine(nameof(SpawnTreeTrunks));
        isTrunkCouroutineRunning = false;
        
        //Desabilita todos os objetos que estão nesse dicionario
        foreach (GameObject obstacle in spawnedObjects.Values)
        {
            obstacle.SetActive(false);
        }
    }

    //Metodo chamado quando o player perder no minigame
    public void PlayerLost()
    {
        //Desativa as Coroutines
        //E reseta seus sinalizadores

        //Desativar primeiro a Coroutina de começar a spawnar, pois ela pode ainda estar em execução caso o player perca no começo
        StopCoroutine(nameof(StartSpawnCoroutine));
        StopCoroutine(nameof(SpawnRocks));
        isRockCouroutineRunning = false;
        StopCoroutine(nameof(SpawnPlants));
        isPlantsCouroutineRunning = false;
        StopCoroutine(nameof(SpawnTreeTrunks));
        isTrunkCouroutineRunning = false;
        
    }

    //Metodo chamado toda vez q o dialogo terminar de ser executado
    private void StartSpawn()
    {
        StartCoroutine(nameof(StartSpawnCoroutine));
    }

    private IEnumerator StartSpawnCoroutine()
    {
        if (!DistanceController.isFirstHalfCompleted)
        {
            StartCoroutine(nameof(SpawnRocks));
            yield return new WaitForSeconds(10f);
            StartCoroutine(nameof(SpawnTreeTrunks));
            yield return new WaitForSeconds(5f);
            StartCoroutine(nameof(SpawnPlants));
        }
        else
        {
            StartCoroutine(nameof(SpawnRocks));
            yield return new WaitForSeconds(3f);
            StartCoroutine(nameof(SpawnPlants));
            yield return new WaitForSeconds(3f);
            StartCoroutine(nameof(SpawnTreeTrunks));
        }
    }

    //Reseta a velocidade de Spawn das pedras
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

    //Pega uma pedra aleatória da lista dos desativados
    private GameObject GetRandomDisabledRock()
    {
        //pega uma pedra aleatoria que ja foi desativada
        int randomIndex = Random.Range(0, disabledRocks.Count);
        //Pega o gameobject da lista
        GameObject obstacle = disabledRocks[randomIndex];
        //Tira da lista
        disabledRocks.RemoveAt(randomIndex);
        return obstacle;
    }

    //Pega um tronco de árvore aleatorio da lista dos desativados
    private GameObject GetRandomDisabledTreeTrunk()
    {
        //pega uma pedra aleatoria que ja foi desativada
        int randomIndex = Random.Range(0, disabledTreeTrunks.Count);
        //Pega o gameobject da lista
        GameObject obstacle = disabledTreeTrunks[randomIndex];
        //Tira da lista
        disabledTreeTrunks.RemoveAt(randomIndex);
        return obstacle;
    }

    //Pega uma planta que empurra para baixo aleatória da lista dos desativados
    private GameObject GetRandomDisabledPlantsDown()
    {
        //pega uma pedra aleatoria que ja foi desativada
        int randomIndex = Random.Range(0, disabledPlantsDown.Count);
        //Pega o gameobject da lista
        GameObject obstacle = disabledPlantsDown[randomIndex];
        //Tira da lista
        disabledPlantsDown.RemoveAt(randomIndex);
        return obstacle;
    }

    //Pega uma planta que empurra para cima aleatória da lista dos desativados
    private GameObject GetRandomDisabledPlantsUp()
    {
        //pega uma pedra aleatoria que ja foi desativada
        int randomIndex = Random.Range(0, disabledPlantsUp.Count);
        //Pega o gameobject da lista
        GameObject obstacle = disabledPlantsUp[randomIndex];
        //Tira da lista
        disabledPlantsUp.RemoveAt(randomIndex);
        return obstacle;
    }
}