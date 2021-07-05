using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerDialogueCommands : MonoBehaviour
{
    [Tooltip("Animator para Porã")]
    public Animator anim;
    [Tooltip("Animator para os simbolos em cima da cabeça de Porã")]
    public Animator symbolAnimator;

    private PlayerController playerController;
    private DialogueRunner dialogueRunner;

    private bool isPlayerMoving = false;
    private float t;
    private Vector3 startPosition;
    private Vector3 target;
    private Vector2 direction;
    private float timeToReachTarget = 3f;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (dialogueRunner == null)
            dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueRunner.IsDialogueRunning == true)
        {
            anim.SetBool("PlayerMoving", isPlayerMoving);
            anim.SetFloat("MoveX", direction.x);
            anim.SetFloat("MoveY", direction.y);

            //Faz o NPC andar, se ele não tiver chegado no seu objetivo
            if (isPlayerMoving)
            {
                if (transform.position != target)
                {
                    t += Time.deltaTime / timeToReachTarget;
                    transform.position = Vector3.Lerp(startPosition, target, t);
                }
                else
                {
                    anim.SetFloat("LastMoveX", direction.y);
                    anim.SetFloat("LastMoveY", direction.y);
                    startPosition = Vector3.zero;
                    target = Vector3.zero;
                    isPlayerMoving = false;
                }
            }
        }
    }

    /// <summary>
    /// Metodo para fazer o NPC andar até a posição target
    /// </summary>
    /// <param name="targetToMove"></param>
    [YarnCommand("movePlayerTo")]
    public void MovePlayerTo(string stringPositionID, string stringTimeToMove)
    {
        if (int.TryParse(stringPositionID, out var positionID) == false)
        {
            Debug.LogErrorFormat($"<<teleportPlayer>> failed to parse position id {stringPositionID}");
            return;
        }

        if (float.TryParse(stringTimeToMove,
                           System.Globalization.NumberStyles.AllowDecimalPoint,
                           System.Globalization.CultureInfo.InvariantCulture,
                           out var timeToMove) == false)
        {
            Debug.LogErrorFormat($"<<moveTo>> failed to parse time to reach target{stringTimeToMove}");
            return;
        }

        
        //Atualiza as variaveis para fazer o NPC andar
        startPosition = transform.position;
        //Pega a posição para onde vai se mover
        target = InstancesManager.singleton.GetNPCManager().GetPoraScenePosition(positionID);

        //Direção calculado através da subtração de alvo menos onde você está
        direction = new Vector2(target.x - startPosition.x, target.y - startPosition.y);
        direction.Normalize();

        timeToReachTarget = timeToMove;

        t = 0;
        isPlayerMoving = true;
    }

    //Metodo a ser chamado toda vez que acabar um minigame
    [YarnCommand("stopMinigame")]
    public void StopPlayingMinigame()
    {
        playerController.StopPlayingMinigame();
    }

    [YarnCommand("sit")]
    public void SitPlayer()
    {
        playerController.SitPlayer();
    }

    [YarnCommand("lookToSide")]
    public void LookToSide(string side)
    {
        print("achou");
        float lastMoveX = 0f;
        float lastMoveY = 0f;

        switch (side)
        {
            case "up":
                lastMoveY = 1f;
                break;
            case "down":
                lastMoveY = -1f;
                break;
            case "right":
                lastMoveX = 1f;
                break;
            case "left":
                print("ue");
                lastMoveX = -1f;
                break;
            default:
                Debug.LogErrorFormat($"<<lookToSide>> failed to parse duration {side}");
                return;
        }

        anim.SetFloat("LastMoveX", lastMoveX);
        anim.SetFloat("LastMoveY", lastMoveY);
        //playerController.ChangeLastMove(lastMoveX, lastMoveY);
    }

    [YarnCommand("popSymbol")]
    public void PopSymbol(string symbolName)
    {
        switch (symbolName)
        {
            case "exclamation":
                symbolAnimator.SetTrigger("Exclamation");
                break;
            default:
                Debug.LogErrorFormat($"<<popSymbol>> failed to findo symbol with name: {symbolName}");
                break;
        }
    }
}
