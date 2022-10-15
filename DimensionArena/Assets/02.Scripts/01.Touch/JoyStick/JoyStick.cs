using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PlayerSpace;

public class JoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // public 전환 , 생성과 동시에 설정 해줘야 한다.
    [SerializeField] public Player player;

    [SerializeField] private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField, Range(10f, 150f)] private float leverRange;

   
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        var inputDir = eventData.position - rectTransform.anchoredPosition;

        var clampedDir = inputDir.magnitude < leverRange ?
            inputDir : inputDir.normalized * leverRange;

        lever.anchoredPosition = clampedDir;
        SetDirection();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        var inputDir = eventData.position - rectTransform.anchoredPosition;
        var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;

        lever.anchoredPosition = clampedDir;
        SetDirection();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        SetDirection();
    }

    public virtual void SetDirection()
    {
        player.direction 
            = new Vector3( (lever.position.x - rectTransform.position.x) * 0.01f, 0, (lever.position.y - rectTransform.position.y) * 0.01f);
    }


    public void DisActiveJoyStick() 
    {
        gameObject.SetActive(false);
    }
}
