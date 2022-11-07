using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PlayerSpace;


public abstract class BaseJoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private enum Direction
    { 
        LEFTUP,
        RIGHTUP,
        LEFTDOWN,
        RIGHTDOWN
    }


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
    [SerializeField, Range(0f, 150f)] protected float leverRange;
    protected float reverseLeverRange;
    protected JoyStickType type;
    [SerializeField] private GameObject[] lightRings;

    [SerializeField] protected float cancelRange = 30f;
    protected bool isDragging;
    protected bool isCancel;


    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        reverseLeverRange = 1.0f / leverRange;
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

        isCancel = inputDir.magnitude < cancelRange;
        if (isCancel)
        {
            lever.anchoredPosition = Vector2.zero;
            eventData.Reset();
        }
        else
            lever.anchoredPosition = clampedDir;
        
        CheckLight();
        SetDirection();
    }
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        CheckLight();
        SetDirection();
    }
    public abstract void SetDirection();

    private void CheckLight()
    {
        if (lever.anchoredPosition.magnitude.Equals(0))
        {
            //키기
            for (int i = 0; i < lightRings.Length; ++i)
            {
                    lightRings[i].SetActive(false);
            }
            return;
        }

        Direction direction;
        //오른쪽
        if(lever.anchoredPosition.x > 0)
        {
            //상단
            if (lever.anchoredPosition.y > 0)
                direction = Direction.RIGHTUP;
            else
                direction = Direction.RIGHTDOWN;
        }
        //왼쪽
        else
        {
            //상단
            if (lever.anchoredPosition.y > 0)
                direction = Direction.LEFTUP;
            else
                direction = Direction.LEFTDOWN;
        }



        //키기
        for(int i = 0; i < lightRings.Length; ++i)
        {
            if (direction == (Direction)i)
                lightRings[i].SetActive(true);
            else
                lightRings[i].SetActive(false);
        }
        

    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {

    }
}
