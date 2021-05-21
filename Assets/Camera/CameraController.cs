using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Yarn.Unity;
using System;

public class CameraController : MonoBehaviour
{
    [Serializable]
    public struct CharactersTransform
    {
        /// <summary>
        /// O nome do personagem do jogo.
        /// </summary>
        public string name;

        /// <summary>
        /// A sprite do portratir do personagem a mostrar no dialogo.
        /// </summary>
        public Transform transform;
    }
    public CharactersTransform[] charactersTransform;
    /// <summary>
    /// Dicionario com os Transform dos personagens, com acesso através do nome O(1).
    /// </summary>
    private Dictionary<string, Transform> charactersPositionDict = new Dictionary<string, Transform>();

    private GameObject player;
    private CinemachineVirtualCamera vcam;
    private CinemachinePixelPerfect cmPixelPerfect;
    private float initialDistance;
    private float startDistance;
    private float targetDistance;
    private float t = 0;
    private float timeToReachTarget;
    private bool startZoom = false;

    //Tem que buscar o player no Start, pois ele ainda n foi criado no Awake
    //Instancia do Player está sendo criado no Awake do GameController
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (player is null) player = InstancesManager.singleton.GetPlayerInstance();

        vcam.m_Follow = player.transform;

        //Transforma o array em dict
        foreach (CharactersTransform por in charactersTransform)
        {
            charactersPositionDict.Add(por.name, por.transform);
        }
    }

    private void Update()
    {
        if (startZoom)
        {
            t += Time.deltaTime / timeToReachTarget;
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(startDistance, targetDistance, t);

            if(vcam.m_Lens.OrthographicSize == targetDistance)
            {
                startZoom = false;
            }
        }
    }

    [YarnCommand("setdistance")]
    public  void SetCameraDistance(string distanceString)
    {
        if (float.TryParse(distanceString,
                                   System.Globalization.NumberStyles.AllowDecimalPoint,
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   out var dist) == false)
        {

            Debug.LogErrorFormat($"<<setdistance>> failed to parse distance {distanceString}");
        }
        //Pega a distância em final que a camera se encontra e salva
        initialDistance = Camera.main.orthographicSize;
        //Pega o componente PixelPerfect na camera virtual
        cmPixelPerfect = GetComponent<CinemachinePixelPerfect>();
        //Desativa para poder ter um transição boa do zoom
        cmPixelPerfect.enabled = false;

        //Muda a distância drasticamente
        vcam.m_Lens.OrthographicSize = dist;
    }

    [YarnCommand("resetCamera")]
    public void ResetCamera()
    {
        //Reseta a distancia da camera
        vcam.m_Lens.OrthographicSize = initialDistance;
        //Ativa pois acabou a necessidade de Zoom
        cmPixelPerfect.enabled = true;
    }

    [YarnCommand("zoomCamera")]
    public void ZoomCamera(string[] parameters)
    {
        startZoom = true;
        t = 0;
        startDistance = vcam.m_Lens.OrthographicSize;
        //Pega a distancia que ta no comando e coloca na variavel 'targetDistance'
        string distanceString = parameters[0];
        if (float.TryParse(distanceString,
                                   System.Globalization.NumberStyles.AllowDecimalPoint,
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   out targetDistance) == false)
        {

            Debug.LogErrorFormat($"<<zoomCamera>> failed to parse duration {distanceString}");
        }

        //Pega a distancia que ta no comando e coloca na variavel 'targetDistance'
        string timeString = parameters[1];
        if (float.TryParse(timeString,
                                   System.Globalization.NumberStyles.AllowDecimalPoint,
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   out timeToReachTarget) == false)
        {

            Debug.LogErrorFormat($"<<zoomCamera>> failed to parse duration {timeString}");
        }
    }

    [YarnCommand("resetZoom")]
    public void ResetZoom(string timeString)
    {
        startZoom = true;
        t = 0;
        startDistance = vcam.m_Lens.OrthographicSize;
        targetDistance = initialDistance;

        //Pega o tempo que vai levar para a camera da zoom
        if (float.TryParse(timeString,
                                   System.Globalization.NumberStyles.AllowDecimalPoint,
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   out timeToReachTarget) == false)
        {

            Debug.LogErrorFormat($"<<initialZoom>> failed to parse duration {timeString}");
        }
    }

    [YarnCommand("setTarget")]
    public void SetTarget(string targetName)
    {
        if (charactersPositionDict.ContainsKey(targetName) == false)
        {
            Debug.LogError("O alvo '" + targetName + "' não existe.");
            return;
        }
        vcam.m_Follow = charactersPositionDict[targetName];
    }

    [YarnCommand("resetTarget")]
    public void ResetTarget()
    {
        vcam.m_Follow = player.transform;
    }
}