using UnityEngine;
using UnityEngine.UI;

public class TransitionEffectController : MonoBehaviour
{
    public Image image;
    public float cutOff;

    // Update is called once per frame
    void Update()
    {
        image.material.SetFloat("_CutOff", cutOff);
    }
}
