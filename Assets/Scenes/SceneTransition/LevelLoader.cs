using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using System;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator gameTransition;
    [SerializeField] private Animator miniGameTransition;
    [SerializeField] private Animator sleepTransition;
    [SerializeField] private Animator flashbackTransition;
    [SerializeField] private Animator panelTransition;
    [SerializeField] private Animator narrativeTransition;
    [SerializeField] private Animator simpleFade;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private float fadeTransitionTime = 2f;
    [SerializeField] private float sleepTransitionTime = 5f;
    [SerializeField] private float narrativeTransitionTime = 23f;
    [SerializeField] private SleepEffectController sleepEffectController;
    [SerializeField] private FlashBackEffectController flashBackEffectController;

    private static int transitionValue = 0;
    private PlayerController player;
    private bool isLevelLoading = false;
    private bool isFirstLoad = true;
    private int flashbackTextID = -1;
    private int sleepTextID = -1;

    void OnEnable()
    {
        SceneManager.sceneLoaded += StartNextLevel;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= StartNextLevel;
    }

    /// <summary>
    /// Metodo chamado por outras classes para fazer com que o level seja carregado usando a transição correta
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="transitionType">
    /// 0: gameTransition;
    /// 1: miniGameTransition;
    /// 2: sleepTransition.</param>
    public void LoadNextLevel(string sceneName, int transitionType)
    {
        transitionValue = transitionType;
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        if (isLevelLoading)
        {
            yield break;
        }

        isLevelLoading = true;

        panelTransition.SetTrigger("Start");
        switch (transitionValue)
        {
            //gameTransition
            case 0:
                gameTransition.SetTrigger("Start");
                yield return new WaitForSeconds(transitionTime);
                break;
            case 1:
                miniGameTransition.SetTrigger("Start");
                yield return new WaitForSeconds(fadeTransitionTime);
                break;
            case 2:
                sleepTransition.SetTrigger("Start");
                sleepEffectController.SetTextByID(sleepTextID);
                yield return new WaitForSeconds(sleepTransitionTime);
                break;
            case 3:
                flashbackTransition.SetTrigger("Start");
                flashBackEffectController.SetTextByID(flashbackTextID);
                yield return new WaitForSeconds(sleepTransitionTime);
                break;
            case 4:
                narrativeTransition.SetTrigger("Start");
                yield return new WaitForSeconds(narrativeTransitionTime);
                break;
            default:
                throw new System.NotImplementedException();
        }

        //Evento para sinalizar que começou a carregar para uma nova cena
        LoadSceneEvents.SceneLoading();

        //Faz com que a cena comece a carregar de forma Async
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }

    public void StartNextLevel(Scene scene, LoadSceneMode mode)
    {
        if (isFirstLoad)
        {
            isFirstLoad = false;
            return;
        }
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
        switch (transitionValue)
        {
            //gameTransition
            case 0:
                //Espera 1 segundo pra conseguir posicionar as coisas a tempo
                yield return new WaitForSeconds(1f);
                gameTransition.SetTrigger("End");
                yield return new WaitForSeconds(transitionTime-1f);
                break;
            case 1:
                miniGameTransition.SetTrigger("End");
                yield return new WaitForSeconds(fadeTransitionTime-1f);
                break;
            case 2:
                sleepTransition.SetTrigger("End");
                yield return new WaitForSeconds(sleepTransitionTime-1f);
                break;
            case 3:
                flashbackTransition.SetTrigger("End");
                yield return new WaitForSeconds(sleepTransitionTime-1f);
                break;
            case 4:
                narrativeTransition.SetTrigger("End");
                //Aqui o tempo de transição é normal
                yield return new WaitForSeconds(fadeTransitionTime-1f);
                break;
            default:
                throw new System.NotImplementedException();
        }

        panelTransition.SetTrigger("End");
        //Reseta as variaveis usadas
        transitionValue = 0;
        flashbackTextID = -1;
        sleepTextID = -1;
        isLevelLoading = false;
    }

    IEnumerator BlackScreen()
    {
        if (isLevelLoading)
        {
            yield break;
        }

        isLevelLoading = true;

        panelTransition.SetTrigger("Start");
        simpleFade.SetTrigger("Start");
        yield return new WaitForSeconds(2f);
        panelTransition.SetTrigger("End");
        simpleFade.SetTrigger("End");
        isLevelLoading = false;
    }

    /// <summary>
    /// Metodo a ser chamado pelo main menu quando for carregar a primeira cena dentro do jogo
    /// A transição sera sempre a Sleep
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="stringTextID"></param>
    public void MainMenuTransition(string sceneName, string stringTextID)
    {
        if (int.TryParse(stringTextID, out var textID) == false)
        {

            Debug.LogErrorFormat($"<<changeScene>> failed to parse int value {stringTextID}");
        }

        sleepTextID = textID;
        LoadNextLevel(sceneName, 2);
    }

    [YarnCommand("changeScene")]
    public void ChangeScene(string sceneName, string transitionType, string playerLoadPoint)
    {
        if(player == null)
        {
            player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();
        }
        
        if (int.TryParse(transitionType, out var type) == false)
        {

            Debug.LogErrorFormat($"<<changeScene>> failed to parse int value {transitionType}");
        }

        //Muda a posição onde o player vai spawnar e carrega nova cena
        player.loadPointName = playerLoadPoint;
        LoadNextLevel(sceneName, type);
    }

    [YarnCommand("blackScreen")]
    public void StartBlackScreen()
    {
        StartCoroutine(BlackScreen());
    }

    [YarnCommand("flashbackScene")]
    public void LoadFlashback(string sceneName, string playerLoadPoint, string stringTextID)
    {
        if (player == null)
        {
            player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();
        }

        if (int.TryParse(stringTextID, out var textID) == false)
        {

            Debug.LogErrorFormat($"<<changeScene>> failed to parse int value {stringTextID}");
        }

        //Muda a posição onde o player vai spawnar e carrega nova cena
        player.loadPointName = playerLoadPoint;
        flashbackTextID = textID;
        LoadNextLevel(sceneName, 3);
    }

    [YarnCommand("sleepTransition")]
    public void SleepTransition(string sceneName, string playerLoadPoint, string stringTextID)
    {
        if (player == null)
        {
            player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();
        }

        if (int.TryParse(stringTextID, out var textID) == false)
        {

            Debug.LogErrorFormat($"<<changeScene>> failed to parse int value {stringTextID}");
        }

        //Muda a posição onde o player vai spawnar e carrega nova cena
        player.loadPointName = playerLoadPoint;
        sleepTextID = textID;
        LoadNextLevel(sceneName, 2);
    }
}
