using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using PlayerSpace;

public class AtkJoyStick : BaseJoyStick
{

    protected override void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
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
        if (isCancel)
        {
            isCancel = false;
            return;
        }

        isDragging = true;

        PlayerAttackRPC();
        base.OnEndDrag(eventData);
    }
    public override void SetDirection()
    {        
        player.Attack.direction =
            new Vector3((lever.position.x - rectTransform.position.x) / leverRange, 0, 
                        (lever.position.y - rectTransform.position.y) / leverRange);

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if(isDragging)
        {
            isDragging = false;
            return;
        }

        // 자동공격 루틴 추가
    }

    private void PlayerAttackRPC()
    {
        player.Attack.StartAttack();
    }
}
