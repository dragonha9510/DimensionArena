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
        isDragging = true;
        base.OnBeginDrag(eventData);
        player.Attack.AtkRangeMeshOnOff(true);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        if(isCancel)
        {
            isDragging = false;
            player.Attack.AtkRangeMeshOnOff(false);
            return;
        }

        isDragging = true;
        player.Attack.AtkRangeMeshOnOff(true);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        player.Attack.AtkRangeMeshOnOff(false);

        if (isCancel)
        {
            isDragging = false;
            isCancel = false;
            return;
        }

        if (!player.Attack.Skilling && !player.Attack.AtkDelaying)
        {
            player.Attack.AttackDelayLock();
            PlayerAttackRPC();
        }
        isDragging = false;
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
        if (isDragging && !isCancel)
        {
            isDragging = false;
            return;
        }

        if (player.Attack.Skilling || player.Attack.AtkDelaying)
            return;

        player.Attack.AttackDelayLock();
        player.Attack.AutoAttack();
    }

    private void PlayerAttackRPC()
    {
        player.Attack.StartAttack();
    }
}
