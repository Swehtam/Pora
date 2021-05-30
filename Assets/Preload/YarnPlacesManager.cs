using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPlacesManager : MonoBehaviour
{
    private HashSet<string> _yarnPlaceDone = new HashSet<string>();

    private void Start()
    {
        //Chamar metodo que atualiza esses boleanos no dicion�rio
        UpdatePlaceDialogueDone();
    }

    // Chamado por cada um dos YarnPlace para salvar que o local j� foi completado e n�o iniciar mais
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

    //Pegar no arquivo salvo os lugares que j� ocorreu dialogo
    private void UpdatePlaceDialogueDone()
    {

    }
}
