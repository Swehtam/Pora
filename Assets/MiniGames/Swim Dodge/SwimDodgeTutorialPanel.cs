using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class SwimDodgeTutorialPanel : MonoBehaviour
{
    public UnityEvent onFirstTimeTutorialDone;

    [SerializeField] private List<GameObject> tutorialImages;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    [SerializeField] private GameObject playButton;

    [SerializeField] private SwimDodgeClassesInterface swimDodgeClassesInterface;
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
            onFirstTimeTutorialDone?.Invoke();
            swimDodgeClassesInterface?.minigameDialogue.StartFirstDialogue();
        }

        IsTutorialRunning = false;
    }

    public void GoToNextImage()
    {
        //Se a ultima image aberta foi a penultima, entao some com o botão de próximo
        if (lastImageOpen == (tutorialImages.Count - 2))
        {
            rightButton.SetActive(false);

            //Se for o primeiro tutorial então aparece com o botão de jogar
            if (IsFirstTutorial)
            {
                playButton.SetActive(true);
            }
        }

        //Sempre que selecionar esse metodo ativa o botão da esquerda
        leftButton.SetActive(true);
        //Some com a ultima imagem aberta
        tutorialImages[lastImageOpen].SetActive(false);
        //Depois mostra a próxima imagem
        lastImageOpen++;
        tutorialImages[lastImageOpen].SetActive(true);
    }

    public void GoToPreviousImage()
    {
        //Se a ultima image aberta foi a segunda, entao some com o botão de anterior
        if (lastImageOpen == 1)
        {
            leftButton.SetActive(false);
        }

        //Sempre que selecionar esse metodo ativa o botão da direita e desativa o botão de jogar
        rightButton.SetActive(true);
        playButton.SetActive(false);
        //Some com a ultima imagem aberta
        tutorialImages[lastImageOpen].SetActive(false);
        //Depois mostra a próxima imagem
        lastImageOpen--;
        tutorialImages[lastImageOpen].SetActive(true);
    }
}
