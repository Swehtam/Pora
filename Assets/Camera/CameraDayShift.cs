using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script utilizado nas camera padr�es da Unity para alterar a cor do background baseado no horario do dia
/// Necess�rio nas cenas onde o mapa � menor do que o tamanho da camera
/// </summary>
public class CameraDayShift : MonoBehaviour
{
    public List<Color> backgroundColors = new List<Color>();

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        //Altera a cor do background da Camera baseado no turno do dia
        camera.backgroundColor = backgroundColors[InstancesManager.singleton.GetDayManager().GetIntDayShift()];
    }

}
