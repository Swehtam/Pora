using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPlacesManager : MonoBehaviour
{
    public bool isTesting = false;
    public List<string> placesVisited = new List<string>();
    private HashSet<string> _yarnPlaceDone = new HashSet<string>();

    private void Start()
    {
        //Chamar metodo que atualiza esses boleanos no dicionário
        if (isTesting)
        {
            LoadYarnPlaceDone(placesVisited.ToArray());
        }
    }

    // Chamado por cada um dos YarnPlace para salvar que o local já foi completado e não iniciar mais
    public void YarnPlaceComplete(string yarnPlaceName)
    {
        // Log that the node has been run.
        _yarnPlaceDone.Add(yarnPlaceName);
    }

    //Metodo para saber se o lugar com dialogo ja foi acionado
    public bool ContainsYarnPlace(string yarnPlaceName)
    {
        return _yarnPlaceDone.Contains(yarnPlaceName);
    }

    //Pegar no arquivo salvo os lugares que já ocorreu dialogo
    public void LoadYarnPlaceDone(string[] placesDone)
    {
        foreach (string placeName in placesDone)
        {
            YarnPlaceComplete(placeName);
        }
    }

    public string[] SaveYarnPlaceDone()
    {
        string[] placesAux = new string[_yarnPlaceDone.Count];
        int i = 0;
        foreach (string place in _yarnPlaceDone)
        {
            placesAux[i] = place;
            i++;
        }

        return placesAux;
    }
}
