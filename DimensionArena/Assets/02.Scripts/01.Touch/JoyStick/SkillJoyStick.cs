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
            base.OnEndDrag(eventData);
            player.Skill.OffSkillMesh();
        }
    }

    public override void SetDirection()
    {
        player.Skill.direction =
            new Vector3((lever.position.x - rectTransform.position.x) / leverRange , 0, (lever.position.y - rectTransform.position.y) / leverRange);
    }


    private void AlphaJoyStick()
    {
        GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    private void SetOnSkillLever()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString("#FFCF32", out color))
            lever.GetComponent<Image>().color = color;
    }

    private void SetOffSkillLever()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString("#909090", out color))
            lever.GetComponent<Image>().color = color;
    }

    public void SkillSetFillAmount(float value)
    {
        skillImg.fillAmount = value;

        if (value.AlmostEquals(1.0f, float.Epsilon))
            SetOnSkillLever();
        else
            SetOffSkillLever();
    }

}

