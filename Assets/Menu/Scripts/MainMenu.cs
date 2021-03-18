using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //Mudar para carregar a tela de menu, quando tiver uma
        SceneManager.LoadScene("VilaLobo");
    }

    public void QuitGame()
    {
        //Fechar o jogo
        Debug.Log("Fechou!");
        Application.Quit();
    }
}
