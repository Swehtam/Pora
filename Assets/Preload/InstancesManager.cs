using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Yarn.Unity;

/* Classe criada baseado no padrão de projeto Singleton.
 * Seu intuito é guardar e fornecer instâncias de classes únicas presentes em cada scene do jogo.
 * Diminuindo o número de chamadas do método FindObjectOfType.
 * Caso a instância não exista no singleton, o mesmo irá buscar essa instância e salvar em seu objeto estático.
 */
public class InstancesManager : MonoBehaviour
{
    public static InstancesManager singleton;
    private GameObject player;
    private DialogueRunner dialogueRunner;
    private PlayerSettings playerSettings;
    private Dictionary<string, Renderer> wolfVillageObjectsRenderers = new Dictionary<string, Renderer>();

    void Awake()
    {
        if (singleton != null)
            GameObject.Destroy(singleton);
        else
            singleton = this;

        DontDestroyOnLoad(this);
    }

    //Método responsável por retornar o GameObject do jogador para quem solicitar
    //Caso o GameObject não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por game)
    public GameObject getPlayerInstance()
    {
        if (player == null)
        {
            var aux = FindObjectOfType<PlayerController>();
            //Se não achar é pq essa é a primeira scene a ser carregada no game, então o GameController irá criar o prefab na scene
            if(aux != null)
                player = aux.gameObject;
        }
            
        return player;
    }

    //Método responsável por retornar o DialogueRunner da scene para quem solicitar
    //Caso o DialogueRunner não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por cena)
    public DialogueRunner getDialogueRunnerInstance()
    {
        if (dialogueRunner == null)
            dialogueRunner = FindObjectOfType<DialogueRunner>();

        return dialogueRunner;
    }

    //Método responsável por retornar o PlayerSettings do _preload para quem solicitar
    //Caso o PlayerSettings não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por game)
    public PlayerSettings getPlayerSettingsInstance()
    {
        if (playerSettings == null)
            playerSettings = FindObjectOfType<PlayerSettings>();

        return playerSettings;
    }

    //Método responsável por retornar o Dicionário com o Nome do Objeto e seu Renderer da scene Vila Lobo Sonho para quem solicitar
    //Caso o Dicionário com o Nome do Objeto e seu Renderer da scene Vila Lobo Sonho não exista no singleton, procure pelo mesmo, salve e retorne (isso ocorre uma vez por game)
    public Dictionary<string, Renderer> getWolfVillageObjectsRenderers()
    {
        if (wolfVillageObjectsRenderers == null)
        {
            //Pega todos os renderes dos tiles do mapa
            var tile_renderers = FindObjectsOfType<TilemapRenderer>();
            foreach (TilemapRenderer renderer in tile_renderers)
            {
                wolfVillageObjectsRenderers.Add(renderer.gameObject.name, renderer);
            }

            //Pega todos os Renderes dos NPCS e de Porã
            var sprite_renderers = FindObjectsOfType<SpriteRenderer>();
            foreach (SpriteRenderer renderer in sprite_renderers)
            {
                wolfVillageObjectsRenderers.Add(renderer.gameObject.name, renderer);
            }
        }

        return wolfVillageObjectsRenderers;
    }
}
