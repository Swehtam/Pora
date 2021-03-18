using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSleep : MonoBehaviour
{
    //Variavel que contem o container para as opções se o player quer dormir ou não
    public GameObject sleepContainer;

    //Evento para ser chamado quando mostrar o Container
    public UnityEngine.Events.UnityEvent onContainerShow;

    //Mostrar o container das opções para dormir e para a movimentação do Player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.gameObject.name == "Porã" && !collision.isTrigger)
        {
            sleepContainer.SetActive(true);
            onContainerShow?.Invoke();
        }
    }
}
