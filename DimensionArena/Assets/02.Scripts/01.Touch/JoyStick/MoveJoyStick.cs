using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PlayerSpace;

public class MoveJoyStick : BaseJoyStick
{
    protected override void Awake()
    {
        type = JoyStickType.MOVE;
        base.Awake();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        SetDirection();
    }

    public override void SetDirection()
    {
        player.direction 
            = new Vector3( (lever.position.x - rectTransform.position.x) * 0.01f, 0, (lever.position.y - rectTransform.position.y) * 0.01f);
    }


}
