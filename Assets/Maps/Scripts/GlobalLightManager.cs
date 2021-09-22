using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GlobalLightManager : MonoBehaviour
{
    public List<Color> skyColors = new List<Color>();
    public List<float> skyColorsIntensity = new List<float>();

    private Light2D globalLight;
    private Color currentColor;
    private bool isGettingDark;
    private bool isGettingNormal;
    private float t;
    private readonly float fadeTime = 1f;

    void Start()
    {
        globalLight = GetComponent<Light2D>();
        LightEvents.OnDarkLighting += DarkLight;
        LightEvents.OnNormalLighting += NormalLight;

        DayManager dayManager = InstancesManager.singleton.GetDayManager();
        //Pega o turno do dia
        int dayShift = dayManager.GetIntDayShift();
        //Altera a cor e a intensidade da luz global, baseado no turno do dia
        globalLight.color = skyColors[dayShift];
        currentColor = skyColors[dayShift];
        globalLight.intensity = skyColorsIntensity[dayShift];
    }

    private void OnDisable()
    {
        LightEvents.OnDarkLighting -= DarkLight;
        LightEvents.OnNormalLighting -= NormalLight;
    }

    private void Update()
    {
        if (isGettingDark && t <= 1f)
        {
            t += Time.deltaTime / fadeTime;
            globalLight.color = Color.Lerp(currentColor, Color.black, t);

            if (t >= 1f)
                isGettingDark = false;
        }

        if(isGettingNormal && t <= 1f)
        {
            t += Time.deltaTime / fadeTime;
            globalLight.color = Color.Lerp(Color.black, currentColor, t);

            if (t >= 1f)
                isGettingNormal = false;
        }
    }

    public void DarkLight()
    {
        t = 0f;
        isGettingDark = true;
    }

    public void NormalLight()
    {
        t = 0f;
        isGettingNormal = true;
    }
}
