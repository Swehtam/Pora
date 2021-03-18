using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerController : MonoBehaviour
{
    //Variaveis de velocidade de Porã
    public float moveSpeed;
    public string loadPointName;

    //Variaveis para o movimento de Porã
    private Vector3 moveInput;
    private bool playerMoving;
    private Vector2 lastMove;
    private Joystick joystick;

    //Componentes de Porã
    private Rigidbody2D myRB;
    private Animator anim;

    private DialogueRunner dialogueRunner;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerMoving = false;

        //Inicializa moveInput em 0, para caso naõ tenha Input horizantal ou vertical
        moveInput = new Vector2(0f, 0f);

        if(dialogueRunner == null)
        {
            dialogueRunner = InstancesManager.singleton.getDialogueRunnerInstance();
        }

        //Para toda a movimentação se o Player
        if (dialogueRunner.IsDialogueRunning == true)
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

    public void StopPlayerMovement()
    {
        //Para a animação
        playerMoving = false;
        //Para o movimento do personagem quando aparecer o novo dialogo
        moveInput = new Vector2(0f, 0f);
    }
}
