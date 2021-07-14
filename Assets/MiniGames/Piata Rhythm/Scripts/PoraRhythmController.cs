using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoraRhythmController : MonoBehaviour
{
    public Transform[] poraPositions;

    public SpriteRenderer farmItemSpriteRenderer;
    public Sprite seedsSprite;
    public Sprite waterSprite;

    public CropController cropController;

    private int currentPosition = 0;

    private Animator animator;
    private Coroutine armsUpCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        RhythmEvents.OnNoteHit += NoteHit;
        farmItemSpriteRenderer.sprite = null;
    }

    private void NoteHit(string noteName)
    {
        if(armsUpCoroutine != null)
        {
            StopCoroutine(armsUpCoroutine);
            UpdatePoraPosition();
            armsUpCoroutine = StartCoroutine(StartNoteHit(noteName));
        }
        else
        {
            armsUpCoroutine = StartCoroutine(StartNoteHit(noteName));
        }
    }

    private void UpdatePoraPosition()
    {
        //Se Porã tiver na ultima posição então reseta
        if (currentPosition == poraPositions.Length - 1)
        {
            currentPosition = 0;
            //Olha para baixo
            animator.SetFloat("LookingAt", -1f);
        }
        //Se não acrescenta
        else
        {
            currentPosition++;
        }

        //Se chegar na fileira de baixo de plantas então atualizar Porã para olhar pra cima
        if (currentPosition == poraPositions.Length / 2)
        {
            //Olha para cima
            animator.SetFloat("LookingAt", 1f);
        }

        transform.position = poraPositions[currentPosition].position;
        armsUpCoroutine = null;
    }

    void OnDisable()
    {
        Debug.Log("Desabilitou Pora no Rhythm minigame.");
        RhythmEvents.OnNoteHit -= NoteHit;
    }

    private IEnumerator StartNoteHit(string noteName)
    {
        if (noteName.Equals("seeds"))
        {
            farmItemSpriteRenderer.sprite = seedsSprite;
        }
        else if (noteName.Equals("water"))
        {
            farmItemSpriteRenderer.sprite = waterSprite;
        }

        //Colocar a sprite do objeto que vai ficar nas mãos de Porã
        animator.SetTrigger("ArmsUp");
        cropController.UpdateCropSpriteRenderer(noteName);
        yield return new WaitForSecondsRealtime(1f);

        UpdatePoraPosition();
    }
}
