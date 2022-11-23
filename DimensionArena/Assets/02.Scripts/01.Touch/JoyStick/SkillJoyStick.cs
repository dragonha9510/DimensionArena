using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using PlayerSpace;

public class SkillJoyStick : BaseJoyStick
{
    [SerializeField] Image skillImg;

    protected override void Awake()
    {
        AlphaJoyStick();
        base.Awake();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if(skillImg.fillAmount.Equals(1.0f))
        {
            isDragging = true;
            base.OnBeginDrag(eventData);
            player.Skill.OnSkillMesh();
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (!skillImg.fillAmount.Equals(1.0f))
            return;
        
        base.OnDrag(eventData);

        if(isCancel)
        {
            isDragging = false;
            player.Skill.OffSkillMesh();
            return;
        }

        isDragging = true;
        player.Skill.OnSkillMesh();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (isCancel)
        {
            isDragging = false;
            isCancel = false;
            return;
        }


        if (skillImg.fillAmount.Equals(1.0f))
        {
            player.Attack.AttackLock();
            isDragging = false;
            //방향, 거리
            player.Skill.UseSkill(player.Skill.direction, player.Skill.MaxRange * (player.Skill.direction.magnitude));
            player.Skill.OffSkillMesh();
            base.OnEndDrag(eventData);
        }
    }

    public override void SetDirection()
    {
        player.Skill.direction =
            new Vector3((lever.position.x - rectTransform.position.x) * reverseLeverRange, 0, (lever.position.y - rectTransform.position.y) * reverseLeverRange);
    }


    private void AlphaJoyStick()
    {
        GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    private void SetOnSkillLever()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString("#FFCF32", out color))
        {
            lever.GetComponent<Image>().color = color;
            ColorUtility.TryParseHtmlString("#FFFFFF", out color);
            GetComponent<Image>().color = new Color(1,1,1,1);
        }
    }

    private void SetOffSkillLever()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString("#909090", out color))
        {
            lever.GetComponent<Image>().color = color;
            ColorUtility.TryParseHtmlString("#FFFFFF", out color);
            GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
    }

    public void SkillSetFillAmount(float value)
    {
        skillImg.fillAmount = value;

        if (value.AlmostEquals(1.0f, float.Epsilon))
            SetOnSkillLever();
        else
            SetOffSkillLever();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging && !isCancel)
        {
            isDragging = false;
            return;
        }

        if (!skillImg.fillAmount.Equals(1.0f))
            return;

        player.Attack.AttackLock();

        // 자동공격 루틴 추가
        player.Skill.AutoSkill();
    }
}

