using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEvents : MonoBehaviour
{
    public delegate void DarkLightingHandler();
    public static event DarkLightingHandler OnDarkLighting;
    public delegate void NormalLightingHandler();
    public static event NormalLightingHandler OnNormalLighting;

    public static void DarkLight()
    {
        OnDarkLighting?.Invoke();
    }

    public static void NormalLight()
    {
        OnNormalLighting?.Invoke();
    }
}
