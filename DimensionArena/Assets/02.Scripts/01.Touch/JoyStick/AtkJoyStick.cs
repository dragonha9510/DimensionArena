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

    private int iCnt = 0;

    public override void OnEndDrag(PointerEventData eventData)
    {
        /*Debug.Log(++iCnt + "포워드");
        Debug.Log(player.transform.forward);
        Debug.Log(++iCnt + "디렉션");
        Debug.Log(player.Attack.direction);*/
        player.Attack.AtkRangeMeshOnOff(false);

        if (isCancel)
        {
            isDragging = false;
            isCancel = false;
            return;
        }

        PlayerAttackRPC();
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

        player.Attack.AutoAttack();
    }

    private void PlayerAttackRPC()
    {
        player.Attack.StartAttack();
    }
}
