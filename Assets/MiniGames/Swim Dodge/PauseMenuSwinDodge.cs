using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PauseMenuSwinDodge : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    [SerializeField] private SwimDodgeClassesInterface swimDodgeClassesInterface;

    private PlayerController player;
    private DialogueRunner dialogueRunner;

    // Start is called before the first frame update
    void Start()
    {
        //Usa o singleton para pegar a instância do player
        player = InstancesManager.singleton.GetPlayerInstance().GetComponent<PlayerController>();
        player.PlayingMinigame();

        dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
    }

    public void PauseGame()
    {
        if (!dialogueRunner.IsDialogueRunning)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }
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
        swimDodgeClassesInterface.distanceController.ResetGame();
        swimDodgeClassesInterface.speedController.ResetGame();
        player.loadPointName = "SaidaNadarDesvio";
        player.StoppedPlayingMinigame();
        InstancesManager.singleton.GetLevelLoaderInstance().LoadNextLevel("VilaLobo", 0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        swimDodgeClassesInterface.distanceController.ResetGame();
        swimDodgeClassesInterface.speedController.ResetGame();
        InstancesManager.singleton.GetLevelLoaderInstance().LoadNextLevel("NadarDesvioMiniGame", 1);
    }

    public void ShowTutorial()
    {
        swimDodgeClassesInterface.swimDodgeTutorialPanel.StartTutorial();
    }
}
