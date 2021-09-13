using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SpotLightManager : MonoBehaviour
{
    public List<float> lightIntensity = new List<float>();

    [Header("Intervalor de turnos para a luz ficar ligada")]
    public int minDayShift = -1;
    public int maxDayShift = -1;

    private Light2D spotLight;
    void Start()
    {
        spotLight = GetComponent<Light2D>();
        DayManager dayManager = InstancesManager.singleton.GetDayManager();

        //Desativa todas as luzes de pontos
        spotLight.gameObject.SetActive(false);

        //Se o turno do dia estiver entre o minimo e o máximo para a luz ficar ligada então liga a luz
        if (minDayShift <= dayManager.GetIntDayShift() && dayManager.GetIntDayShift() <= maxDayShift)
        {
            spotLight.gameObject.SetActive(true);
        }

        //Ajusta a intensidade da luz
        spotLight.intensity = lightIntensity[dayManager.GetIntDayShift()];
    }
}
