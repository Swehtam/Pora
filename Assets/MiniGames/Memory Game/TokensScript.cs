using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TokensScript : MonoBehaviour
{
    public int faceIndex;
    public string animalName;
    public MemoryGameController memoryGameController;
    public SpriteRenderer backSpriteRenderer;

    [SerializeField] private bool matched = false;
    [SerializeField] private bool revealed = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMouseDown()
    {
        if (IsPointerOverUIObject())
            return;

        if (matched == false && !memoryGameController.checkingMatch)
        {
            if (!revealed && memoryGameController.TwoTokensUp() == false)
            {
                StartCoroutine(RevealToken());
            }
        }
    }

    IEnumerator RevealToken()
    {
        revealed = true;
        animator.SetTrigger("Reveal");
        //Pega se esse token é o primeiro ou o segundo selecionado
        int tokenNumber = memoryGameController.AddVisibleFace(this);
        //Se for o segundo, então faz a checagem de match
        if (tokenNumber == 1)
        {
            yield return new WaitForSecondsRealtime(1f);
            matched = memoryGameController.CheckMatch();
        }
        yield return null;
    }

    public void StartHideToken()
    {
        StartCoroutine(HideToken());
    }

    IEnumerator HideToken()
    {
        animator.SetTrigger("Hide");
        yield return new WaitForSecondsRealtime(1f);
        revealed = false;
    }

    public IEnumerator ResetToken()
    {
        animator.SetTrigger("Hide");
        yield return new WaitForSecondsRealtime(1.5f);
        faceIndex = 0;
        backSpriteRenderer.sprite = null;
        matched = false;
        revealed = false;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
