using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerController : MonoBehaviour
{
    //Variaveis de velocidade de Porã
    public float moveSpeed;
    public string loadPointName;

    [Tooltip("Animator para Porã")]
    public Animator anim;
    [Tooltip("Sprite Render para ajustar a ordem na cena")]
    public SpriteRenderer playerSpriteRenderer;
    

    //Variaveis para o movimento de Porã
    private Vector3 moveInput;
    private bool playerMoving;
    private Vector2 lastMove;
    private Joystick joystick;
    private bool IsPlayingMinigame = false;

    //Componentes de Porã
    private Rigidbody2D myRB;
    private Collider2D collider;

    //Dialogo
    private DialogueRunner dialogueRunner;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        if (dialogueRunner == null)
            dialogueRunner = InstancesManager.singleton.GetDialogueRunnerInstance();
    }

    // Update is called once per frame
    void Update()
    {
        playerMoving = false;

        //Inicializa moveInput em 0, para caso naõ tenha Input horizantal ou vertical
        moveInput = new Vector2(0f, 0f);

        //Para toda a movimentação se o Player
        if (dialogueRunner.IsDialogueRunning == true || IsPlayingMinigame || joystick == null)
        {
            //Reseta a animação para não bugar
            anim.SetFloat("MoveX", moveInput.x);
            anim.SetFloat("MoveY", moveInput.y);
            anim.SetBool("PlayerMoving", playerMoving);
            return;
        }

        //Seta o moveInput com relação a horizontal, caso tenha
        if (joystick.Horizontal >= .2f)
        {
            moveInput = new Vector2(moveInput.x + 1f, moveInput.y);
        }else if (joystick.Horizontal <= -.2f)
        {
            moveInput = new Vector2(moveInput.x - 1f, moveInput.y);
        }

        //Seta o moveInput com relação a vertical, caso tenha
        if (joystick.Vertical >= .2f)
        {
            moveInput = new Vector2(moveInput.x, moveInput.y + 1f);
        }
        else if (joystick.Vertical <= -.2f)
        {
            moveInput = new Vector2(moveInput.x, moveInput.y - 1f);
        }

        //Normaliza para quando andar nas diagonais nao estar muito rapido
        moveInput.Normalize();
        moveInput *= moveSpeed;

        if (moveInput.x != 0 || moveInput.y != 0)
        {
            playerMoving = true;
            lastMove = new Vector2(moveInput.x, moveInput.y);
        }

        anim.SetFloat("MoveX", moveInput.x);
        anim.SetFloat("MoveY", moveInput.y);
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }

    private void FixedUpdate()
    {
        //Mover o player atraves do teclado caso nao tenha dialogo
        myRB.velocity = moveInput;
    }

    public void SetJoystick(Joystick joy)
    {
        joystick = joy;
    }

    //Metodo a ser chamado toda vez q iniciar um minigame
    public void PlayingMinigame()
    {
        IsPlayingMinigame = true;
    }

    public void TeleportTo(Transform placeTransform)
    {
        gameObject.transform.position = placeTransform.position;
    }

    public void SitPlayer()
    {
        //Deixa o collider como trigger para não bater quando for sentar
        collider.isTrigger = true;
        playerSpriteRenderer.sortingOrder = 1;
        //Look Up
        lastMove = new Vector2(0f, 1f);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
        anim.SetBool("isSitting", true);
    }

    public void NotSiting()
    {
        //Caso não esteja acontecendo dialogo ou minigame
        //Fazer com q o player não esteja sentado e a ordem do layer volte pra 0
        //Voltar ao normal o collider
        collider.isTrigger = false;
        playerSpriteRenderer.sortingOrder = 0;
        anim.SetBool("isSitting", false);
    }

    //Metodo a ser chamado toda vez que acabar um minigame
    public void StopPlayingMinigame()
    {
        IsPlayingMinigame = false;
    }

    public void ChangeLastMove(float lastMoveX, float lastMoveY)
    {
        lastMove = new Vector2(lastMoveX, lastMoveY);
        anim.SetFloat("LastMoveX", lastMoveX);
        anim.SetFloat("LastMoveY", lastMoveY);
    }
}
