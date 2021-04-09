using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimDodgeTutorialPanel : MonoBehaviour
{
    [SerializeField] private List<GameObject> tutorialImages;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    [SerializeField] private GameObject playButton;
    [SerializeField] private MinigameClassesInterface minigameClassesInterface;
    private int lastImageOpen = 0;
    
    public static bool IsFirstTutorial = true;
    public static bool IsTutorialRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        if (IsFirstTutorial)
        {
            StartTutorial();
        }
    }

    public void StartTutorial()
    {
        IsTutorialRunning = true; //Variavel para saber se o tutorial está rodando

        tutorialPanel.SetActive(true); //Desativar o painel do tutorial
        rightButton.SetActive(true); //Aparecer o botão para passar para a próxima imagem
        leftButton.SetActive(false); //Sumir com o botão de voltar
        playButton.SetActive(false); //Sumir com o botão de iniciar o jogo
        tutorialImages[0].SetActive(true); //Mostra a primeira imagem do tutorial
        lastImageOpen = 0; //Ultima imagem aberta foi a primeira
    }

    public void PlayOrCloseButton()
    {
        //Desativa tudo
        tutorialImages[lastImageOpen].SetActive(false);
        rightButton.SetActive(false);
        leftButton.SetActive(false);
        playButton.SetActive(false);
        tutorialPanel.SetActive(false);

        //Se for o primeiro tutorial
        if (IsFirstTutorial)
        {
            //Sinaliza que o tutorial acabou
            IsFirstTutorial = false;
            minigameClassesInterface.minigameDialogue.StartFirstDialogue();
        }

        IsTutorialRunning = false;
    }

    public void GoToNextImage()
    {
        //Se a ultima image foi a primeira, então mostrar o botão para voltar
        if (lastImageOpen == 0)
        {
            leftButton.SetActive(true);
        }
        //Se a ultima image aberta foi a penultima, entao some com o botão de próximo
        else if (lastImageOpen == tutorialImages.Count - 2)
        {
            rightButton.SetActive(false);

            //Se for o primeiro tutorial então aparece com o botão de jogar
            if (IsFirstTutorial)
            {
                playButton.SetActive(true);
            }
        }

        //Some com a ultima imagem aberta
        tutorialImages[lastImageOpen].SetActive(false);
        //Depois mostra a próxima imagem
        lastImageOpen++;
        tutorialImages[lastImageOpen].SetActive(true);
    }

    public void GoToPreviousImage()
    {
        //Se a ultima imagem aberta foi a ultima da lista, então mostra o botão de próximo
        if (lastImageOpen == tutorialImages.Count - 1)
        {
            rightButton.SetActive(true);

            //Se for o primeiro tutorial então aparece com o botão de jogar
            if (IsFirstTutorial)
            {
                playButton.SetActive(false);
            }
        }
        //Se a ultima image aberta foi a segunda, entao some com o botão de anterior
        else if (lastImageOpen == 1)
        {
            leftButton.SetActive(false);
        }

        //Some com a ultima imagem aberta
        tutorialImages[lastImageOpen].SetActive(false);
        //Depois mostra a próxima imagem
        lastImageOpen--;
        tutorialImages[lastImageOpen].SetActive(true);
    }
}
