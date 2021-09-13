using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class FlashbackAnimations : MonoBehaviour
{
    public Animator toysAnimatior;

    [YarnCommand("showToys")]
    public void ShowToys()
    {
        toysAnimatior.SetTrigger("showToys");
    }

    [YarnCommand("showCapivara")]
    public void ShowCapivara()
    {
        toysAnimatior.SetTrigger("showCapivara");
    }
}
