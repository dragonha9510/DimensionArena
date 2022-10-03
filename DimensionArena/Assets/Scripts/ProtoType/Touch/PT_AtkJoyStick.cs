using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PT_AtkJoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] public Player_Atk player;

    [SerializeField] private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField, Range(10f, 150f)] private float leverRange;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var realpos = new Vector2(Screen.width - rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

        var inputDir = eventData.position - new Vector2(rectTransform.position.x, rectTransform.position.y);

        var clampedDir = inputDir.magnitude < leverRange ?
            inputDir : inputDir.normalized * leverRange;

        lever.anchoredPosition = clampedDir;
        SetDirection();
    }

    public void OnDrag(PointerEventData eventData)
    {
        var realpos = new Vector2(Screen.width - rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

        var inputDir = eventData.position - new Vector2(rectTransform.position.x, rectTransform.position.y);
        var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;

        lever.anchoredPosition = clampedDir;
        SetDirection();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        SetDirection();
        player.Attack();
    }

    public void SetDirection()
    {
        player.direction = new Vector3((lever.position.x - rectTransform.position.x) * 0.01f, 0, (lever.position.y - rectTransform.position.y) * 0.01f);
    }
}
