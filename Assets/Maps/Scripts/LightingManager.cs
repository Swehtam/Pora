using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LightingManager : MonoBehaviour
{
    [YarnCommand("darkenTheScene")]
    public void DarkenTheScene()
    {
        LightEvents.DarkLight();
    }

    [YarnCommand("lightenTheScene")]
    public void LightenTheScene()
    {
        LightEvents.NormalLight();
    }
}
