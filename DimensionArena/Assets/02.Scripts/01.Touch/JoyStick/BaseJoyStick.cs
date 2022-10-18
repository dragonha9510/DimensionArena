using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PlayerSpace;


public abstract class BaseJoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected enum JoyStickType
    { 
        MOVE,
        ATTACK,
        SKILL,
        JOYSTICK_END
    }
    [SerializeField] public Player player;
    [SerializeField] protected RectTransform lever;
    protected RectTransform rectTransform;
    [SerializeField, Range(10f, 150f)] protected float leverRange;
    protected JoyStickType type;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        var realpos = new Vector2(Screen.width - rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

        var inputDir = eventData.position - new Vector2(rectTransform.position.x, rectTransform.position.y);

        var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;

        lever.anchoredPosition = clampedDir;
        SetDirection();
    }
    public virtual void OnDrag(PointerEventData eventData)
    {
        var realpos = new Vector2(Screen.width - rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

        var inputDir = eventData.position - new Vector2(rectTransform.position.x, rectTransform.position.y);
        var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;

        lever.anchoredPosition = clampedDir;
        SetDirection();
    }
    public abstract void OnEndDrag(PointerEventData eventData);
    public abstract void SetDirection();
}
