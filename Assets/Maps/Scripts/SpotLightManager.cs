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
    private bool isGettingDark;
    private bool isGettingNormal;
    private float t;
    private readonly float fadeTime = 1f;

    void Start()
    {
        spotLight = GetComponent<Light2D>();
        LightEvents.OnDarkLighting += SpotDarkLight;
        LightEvents.OnNormalLighting += SpotNormalLight;

        DayManager dayManager = InstancesManager.singleton.GetDayManager();
        //Se o turno do dia for menor do que o minimo ou maior do que o maximo desliga a luz
        if (minDayShift > dayManager.GetIntDayShift() && dayManager.GetIntDayShift() > maxDayShift)
        {
            spotLight.gameObject.SetActive(false);
        }

        //Ajusta a intensidade da luz
        spotLight.intensity = lightIntensity[dayManager.GetIntDayShift()];
    }

    private void OnDisable()
    {
        LightEvents.OnDarkLighting -= SpotDarkLight;
        LightEvents.OnNormalLighting -= SpotNormalLight;
    }

    private void Update()
    {
        if (isGettingDark && t <= 1f)
        {
            t += Time.deltaTime / fadeTime;
            spotLight.color = Color.Lerp(Color.white, Color.black, t);

            if (t >= 1f)
                isGettingDark = false;
        }

        if (isGettingNormal && t <= 1f)
        {
            t += Time.deltaTime / fadeTime;
            spotLight.color = Color.Lerp(Color.black, Color.white, t);

            if (t >= 1f)
                isGettingNormal = false;
        }
    }

    public void SpotDarkLight()
    {
        t = 0f;
        isGettingDark = true;
    }

    public void SpotNormalLight()
    {
        t = 0f;
        isGettingNormal = true;
    }
}
