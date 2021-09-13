using UnityEngine;
using Yarn.Unity;
using System;

/// attached to the non-player characters, and stores the name of the Yarn
/// node that should be run when you talk to them.

public class YarnNPC : MonoBehaviour
{
    public string characterName = "";

    public string talkToNode = "";

    public YarnProgram scriptToLoad;

    private bool isMoving = false;
    private float t;
    private Vector3 startPosition;
    private Vector3 target;
    private Vector2 direction;
    private float timeToReachTarget = 3f;
    private UIManager uIManager;
    private Animator animator;

    void Start()
    {
        if (scriptToLoad != null)
        {
            DialogueRunner dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
            dialogueRunner.Add(scriptToLoad);
        }

        if(uIManager == null)
            uIManager = InstancesManager.singleton.GetUIManager();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Testar se posso mover esse bloco para ficar abaixo do bloco abaixo
        if(animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
        }

        //Faz o NPC andar, se ele não tiver chegado no seu objetivo
        if (isMoving)
        {
            if(transform.position != target)
            {
                t += Time.deltaTime / timeToReachTarget;
                transform.position = Vector3.Lerp(startPosition, target, t);
            }
            else
            {
                startPosition = Vector3.zero;
                target = Vector3.zero;
                isMoving = false;
            }
        }
    }

    /// <summary>
    /// Metodo para fazer o NPC andar até a posição target
    /// </summary>
    /// <param name="targetToMove"></param>
    public void MoveTo(Vector3 targetToMove, float timeToMove)
    {
        //Atualiza as variaveis para fazer o NPC andar
        startPosition = transform.position;
        target = targetToMove;
        direction = new Vector2(target.x - startPosition.x, target.y - startPosition.y);
        direction.Normalize();
        timeToReachTarget = timeToMove;
        t = 0;
        isMoving = true;
    }

    [YarnCommand("lookToSide")]
    public void LookToSide(string side)
    {
        animator.SetFloat("LastMoveX", 0f);
        animator.SetFloat("LastMoveY", 0f);
        switch (side)
        {
            case "up":
                animator.SetFloat("LastMoveY", 1f);
                break;
            case "down":
                animator.SetFloat("LastMoveY", -1f);
                break;
            case "right":
                animator.SetFloat("LastMoveX", 1f);
                break;
            case "left":
                animator.SetFloat("LastMoveX", -1f);
                break;
            default:
                Debug.LogErrorFormat($"<<lookToSide>> failed to parse duration {side}");
                break;
        }
    }

    [YarnCommand("kneel")]
    public void Kneel()
    {
        animator.SetBool("IsOnKnees", true);
    }

    [YarnCommand("standUp")]
    public void StandUp()
    {
        animator.SetBool("IsOnKnees", false);
        //Colocar depois caso precise
        //animator.SetBool("IsSitting", false);
    }

    [YarnCommand("isAngry")]
    public void IsAngry()
    {
        animator.SetBool("IsAngry", true);
    }

    [YarnCommand("isNotAngry")]
    public void IsNotAngry()
    {
        animator.SetBool("IsAngry", false);
    }

    [YarnCommand("disableNPC")]
    public void DisableNPC()
    {
        gameObject.SetActive(false);
    }

    //Quando o NPC encontrar o Player mostrar o botão para falar
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            uIManager.ShowTalkButton(this);
    }

    //Usado para saber se o player saio de perto do NPC
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            uIManager.HideTalkButton();
    }
}
