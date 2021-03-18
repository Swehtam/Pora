using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    void Start()
    {
        //Mudar para carregar a tela de menu, quando tiver uma
        SceneManager.LoadScene("MainMenu");
    }
}
