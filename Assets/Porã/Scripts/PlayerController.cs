using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variaveis de velocidade de Porã
    public float moveSpeed;

    //Variaveis para o movimento de Porã
    private Vector3 moveInput;
    private bool playerMoving;
    private Vector2 lastMove;

    //Componentes de Volstagg
    private Rigidbody2D myRB;
    private Animator anim;

    public static bool playerExists;

    public string loadPointName;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponentInChildren<Animator>();

        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(transform.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerMoving = false;

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveInput.Normalize();
        moveInput *= moveSpeed;

        if (moveInput.x != 0 || moveInput.y != 0)
        {
            playerMoving = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }

    private void FixedUpdate()
    {
        //Mover o player atraves do teclado caso nao tenha dialogo
        myRB.velocity = moveInput;
    }
}
