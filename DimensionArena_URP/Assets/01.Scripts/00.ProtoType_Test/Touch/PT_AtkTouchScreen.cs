using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PT_TouchScreen : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private PT_JoyStick joyStick;
    [SerializeField] private RectTransform moveJoyStick;
    private Vector2 oriPosition;

    private void Awake()
    {
        oriPosition = moveJoyStick.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        moveJoyStick.position = eventData.position;
        joyStick.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        joyStick.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        moveJoyStick.position = oriPosition;
        joyStick.OnEndDrag(eventData);
    }
}
