using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CinemachineSwitcher : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    [YarnCommand("changeCamera")]
    public void CameraSwitch(string cameraName)
    {
        animator.Play(cameraName);
    }
}
