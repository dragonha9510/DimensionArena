using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using PlayerSpace;

public class SkillJoyStick : BaseJoyStick
{
    [SerializeField] Image[] alphaImage = new Image[3];
    [SerializeField] Image skillImg;

    protected override void Awake()
    {
        base.Awake();
        AlphaJoyStick();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if(skillImg.fillAmount.Equals(1.0f))
        {
            base.OnBeginDrag(eventData);
            player.Skill.OnSkillMesh();
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (skillImg.fillAmount.Equals(1.0f))
            base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (skillImg.fillAmount.Equals(1.0f))
        {
            lever.anchoredPosition = Vector2.zero;
            SetDirection();
            player.Skill.OffSkillMesh();
        }
    }

    public override void SetDirection()
    {
        player.Skill.direction =
            new Vector3((lever.position.x - rectTransform.position.x) / leverRange , 0, (lever.position.y - rectTransform.position.y) / leverRange);
    }


    //옮길 예정 조이스틱의 상태를 관리해주는 클래스 제작하여 동시터치도 막아야하나?
    public void AlphaJoyStick()
    {
       
    }

    public void MaxSkillPoint()
    {
        alphaImage[0].color = new Color(1, 0.1367925f, 0.1367925f, 1);

        for (int i = 1; i < 3; ++i)
        {
            alphaImage[i].color = 
                new Color(alphaImage[i].color.r, alphaImage[i].color.g, alphaImage[i].color.b, 180 / 255f);
        }

    }

    public void SkillSetFillAmount(float value)
    {
        skillImg.fillAmount = value;

        if(value.AlmostEquals(1.0f,float.Epsilon))
        {
            MaxSkillPoint();
        }
    }

}

