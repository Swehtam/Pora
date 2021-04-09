using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuSwinDodge : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    [SerializeField] private MinigameClassesInterface minigameClassesInterface;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        //Usa o singleton para pegar a instância do player
        player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();
        player.PlayingMinigame();
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        minigameClassesInterface.distanceController.ResetGame();
        minigameClassesInterface.speedController.ResetGame();
        player.loadPointName = "Saida Nadar Desvio";
        player.StoppedPlayingMinigame();
        SceneManager.LoadScene("VilaLobo");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        minigameClassesInterface.distanceController.ResetGame();
        minigameClassesInterface.speedController.ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowTutorial()
    {
        minigameClassesInterface.swimDodgeTutorialPanel.StartTutorial();
    }
}
