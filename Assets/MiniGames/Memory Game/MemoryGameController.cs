using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MemoryGameController : MonoBehaviour
{
    [Serializable]
    public struct BiologyImages
    {
        /// <summary>
        /// Sprite da imagem da aula de biologia
        /// </summary>
        public  Sprite image;
        /// <summary>
        /// Numero do indice da sprite
        /// </summary>
        public int index;
        /// <summary>
        /// String com o nome do Animal
        /// </summary>
        public string animal_name;
    }
    public BiologyImages[] arrayBiologyImages;
    public List<GameObject> tokensEasy = new List<GameObject>();
    public static System.Random rnd = new System.Random();
    public bool checkingMatch = false;

    [SerializeField] private MemoryGameClassesInterface memoryGameClassesInterface;

    private List<int> faceIndexes = new List<int>();
    private TokensScript[] visibleTokens = { null, null};
    private int matchesDone = 0;
    private readonly int neededMatches = 12;
    private int phaseLevel = 1;

    //Dicionário pra salvar o indice e a sprite das imagens para ter acesso mais rapido
    private Dictionary<int, (Sprite, string)> biologyImages = new Dictionary<int, (Sprite, string)>();
    private Dictionary<int, bool> chosenImages = new Dictionary<int, bool>();

    void Start()
    {
        //Mudar o total de matches necessarios a partir da classe MinigamesManager

        InitBiologyImagesDict();
        SelectBiologyImages();
        //Coloquei 0, pois ainda não tem sistema de dificuldade
        StartNewPhase(0);
        RemoveUsedIndexes();

        memoryGameClassesInterface.memoryGameUI.DisplayPhaseLevel(phaseLevel);

        if (!SwimDodgeTutorialPanel.IsFirstTutorial)
            memoryGameClassesInterface.timer.StartTimer(150);
    }

    public bool TwoTokensUp()
    {
        bool cardsUp = false;
        if (visibleTokens[0] != null && visibleTokens[1] != null)
        {
            cardsUp = true;
        }
        return cardsUp;
    }

    public int AddVisibleFace(TokensScript token)
    {
        if (visibleTokens[0] == null)
        {
            visibleTokens[0] = token;
            return 0;
        }
        else if (visibleTokens[1] == null)
        {
            visibleTokens[1] = token;
            //Coloca aqui pois é a segunda opção do player
            checkingMatch = true;
            return 1;
        }

        return -1;
    }

    public bool CheckMatch()
    {
        bool success = false;
        if (visibleTokens[0].faceIndex == visibleTokens[1].faceIndex)
        {
            //Chama o evento de que ocorreu um match e passa o nome do animal como parametro
            MemoryGameEvents.TokenMatch(visibleTokens[0].animalName);

            //Reseta as variaveis que seguram os tokens
            visibleTokens[0] = null;
            visibleTokens[1] = null;

            //Seta que não está mais checando match
            checkingMatch = false;

            //Chama o metodo que está cotrolando a adição de matches
            AddMatches();

            //Retorna sucesso
            success = true;
            return success;
        }

        RemoveVisibleFaces();
        return success;
    }

    public void RemoveVisibleFaces()
    {
        //Reseta o primeiro
        visibleTokens[0].StartHideToken();
        visibleTokens[0] = null;

        //Reseta o segundo
        visibleTokens[1].StartHideToken();
        visibleTokens[1] = null;

        checkingMatch = false;
    }

    public void FinishMinigame()
    {
        //Para o tempo
        memoryGameClassesInterface.timer.StopTimer();

        //Calcula a média
        float results = ((float)matchesDone / neededMatches) * 100f;
        memoryGameClassesInterface.memoryGameUI.ShowFinishingPanel(results);
    }

    /// <summary>
    /// Método para instanciar indices e sprites no dicionário de imagens de biologia
    /// Através das variaveis preenchidas no inspector
    /// </summary>
    private void InitBiologyImagesDict()
    {
        foreach (BiologyImages bio in arrayBiologyImages)
        {
            biologyImages.Add(bio.index, (bio.image, bio.animal_name));
        }
    }

    /// <summary>
    /// Metodo para selecionar as imagens para a nova fase
    /// </summary>
    private void SelectBiologyImages()
    {
        int i = 0;
        List<int> imagesIndexes = new List<int>(biologyImages.Keys);
        int size = imagesIndexes.Count;
        //Seleciona só 4 diferentes pq é a fase mais fácil, tornar isso aqui variavel com a dificuldade
        while (i < 4)
        {
            int shuffleNum = rnd.Next(0, size);
            if (chosenImages.ContainsKey(imagesIndexes[shuffleNum]) == false)
            {
                //Adiciona 2x, pq é um par que vai estar na cena
                faceIndexes.Add(imagesIndexes[shuffleNum]);
                faceIndexes.Add(imagesIndexes[shuffleNum]);
                chosenImages.Add(imagesIndexes[shuffleNum], true);
                i++;
            }
        }
    }

    /// <summary>
    /// Metodo para instanciar os tokens no jogo
    /// </summary>
    /// <param name="difficulty"></param>
    private void StartNewPhase(int difficulty)
    {
        //Quantidade atual de images para a fase
        int originalLength = faceIndexes.Count;
        for (int i = 0; i < originalLength; i++)
        {
            //Pega uma imagem aleatória
            int shuffleNum = rnd.Next(0, (faceIndexes.Count));

            //Atualiza o Token Script nos tokens já presentes na cena
            //Seta como ativo
            tokensEasy[i].SetActive(true);

            //Seta no script do token seu indice, sua sprite e o controlador do jogo
            tokensEasy[i].GetComponent<TokensScript>().faceIndex = faceIndexes[shuffleNum];
            tokensEasy[i].GetComponent<TokensScript>().backSpriteRenderer.sprite = biologyImages[faceIndexes[shuffleNum]].Item1;
            tokensEasy[i].GetComponent<TokensScript>().animalName = biologyImages[faceIndexes[shuffleNum]].Item2;
            tokensEasy[i].GetComponent<TokensScript>().memoryGameController = this;

            //Remove indice da imagem do token que acabou de ser instanciado da lista de possiveis imagens
            faceIndexes.Remove(faceIndexes[shuffleNum]);
        }
    }

    /// <summary>
    /// Remove tokens que já foram instanciados na fase
    /// </summary>
    private void RemoveUsedIndexes()
    {
        foreach (int index in chosenImages.Keys)
        {
            biologyImages.Remove(index);
        }
        //Limpa o dicionário com as imagens já escolhidas
        chosenImages.Clear();
    }

    private void AddMatches()
    {
        matchesDone++;
        //Quando chegar na quantidade de matches maxima na fase, então pula pra nova fase
        if (matchesDone % 4 == 0)
        {
            //Pasou de fase, então mostrar no display nova fase
            phaseLevel++;
            if(phaseLevel <= 3)
                memoryGameClassesInterface.memoryGameUI.DisplayPhaseLevel(phaseLevel);

            if (matchesDone < neededMatches)
            {
                StartCoroutine(StartingNewPhase());
            }
            else
            {
                FinishMinigame();
                Debug.Log("Minigame Completed.");
            }
        }
    }

    /// <summary>
    /// Metodo para resetar todos os tokens no mapa
    /// </summary>
    private void ClearTokens()
    {
        foreach(GameObject token in tokensEasy)
        {
            StartCoroutine(token.GetComponent<TokensScript>().ResetToken());
        }
    }

    IEnumerator StartingNewPhase()
    {
        //Limpa os tokens que estão no mapa
        ClearTokens();
        yield return new WaitForSecondsRealtime(1.5f);
        //Inicia nova fase
        SelectBiologyImages();
        StartNewPhase(0);
        RemoveUsedIndexes();
    }
}