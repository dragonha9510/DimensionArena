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
        player.Attack.AtkRangeMeshOnOff(true);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        player.Attack.AtkRangeMeshOnOff(!isCancel);

        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        player.Attack.AtkRangeMeshOnOff(false);

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
        player.Attack.Calculate();
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

        player.Attack.AutoAttack();
    }

    private void PlayerAttackRPC()
    {
        player.Attack.StartAttack();
    }
}
