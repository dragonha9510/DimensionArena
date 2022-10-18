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
        lever.anchoredPosition = Vector2.zero;
        PlayerAttackRPC();
        SetDirection();
    }
    public override void SetDirection()
    {        
        player.Attack.direction =
            new Vector3((lever.position.x - rectTransform.position.x) / leverRange, 0, 
                        (lever.position.y - rectTransform.position.y) / leverRange);

    }

    private void PlayerAttackRPC()
    {
        player.Attack.StartAttack();
    }
}
