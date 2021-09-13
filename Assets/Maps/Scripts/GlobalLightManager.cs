using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GlobalLightManager : MonoBehaviour
{
    public List<Color> skyColors = new List<Color>();
    public List<float> skyColorsIntensity = new List<float>();

    private Light2D globalLight;
    
    void Start()
    {
        globalLight = GetComponent<Light2D>();
        DayManager dayManager = InstancesManager.singleton.GetDayManager();

        //Pega o turno do dia
        int dayShift = dayManager.GetIntDayShift();
        //Altera a cor e a intensidade da luz global, baseado no turno do dia
        globalLight.color = skyColors[dayShift];
        globalLight.intensity = skyColorsIntensity[dayShift];
    }
}
