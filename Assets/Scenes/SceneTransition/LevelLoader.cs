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
    [SerializeField] private Animator panelTransition;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private float fadeTransitionTime = 2f;
    [SerializeField] private float sleepTransitionTime = 5f;
    [SerializeField] private SleepEffectController sleepEffectController;

    private static int transitionValue = 0;
    private PlayerController player;

    private void OnLevelWasLoaded()
    {
        StartNextLevel();
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
                sleepEffectController.AutoSetText();
                yield return new WaitForSeconds(sleepTransitionTime);
                break;
            default:
                throw new System.NotImplementedException();
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }

    public void StartNextLevel()
    {
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
        panelTransition.SetTrigger("End");
        switch (transitionValue)
        {
            //gameTransition
            case 0:
                gameTransition.SetTrigger("End");
                yield return new WaitForSeconds(transitionTime);
                break;
            case 1:
                miniGameTransition.SetTrigger("End");
                yield return new WaitForSeconds(fadeTransitionTime);
                break;
            case 2:
                sleepTransition.SetTrigger("End");
                yield return new WaitForSeconds(sleepTransitionTime);
                break;
            default:
                throw new System.NotImplementedException();
        }
        //Reseta para 0 para usar a transição padrão
        transitionValue = 0;        
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
}
