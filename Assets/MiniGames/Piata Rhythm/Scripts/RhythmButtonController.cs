using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RhythmButtonController : MonoBehaviour
{
    public string noteName;

    public Color defaultColor;
    public Color pressedColor;

    public NoteObject noteObject;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        if (IsPointerOverUIObject())
            return;

        spriteRenderer.color = pressedColor;

        if (noteObject != null)
        {
            noteObject.NoteHited(noteName);
            noteObject = null;
        }
    }

    public void OnMouseUp()
    {
        if (IsPointerOverUIObject())
            return;

        spriteRenderer.color = defaultColor;
    }

    public void NoteEntered(NoteObject note)
    {
        noteObject = note;
    }

    public void NoteExited()
    {
        noteObject = null;
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
