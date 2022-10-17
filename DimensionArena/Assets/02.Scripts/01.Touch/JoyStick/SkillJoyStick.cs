using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using PlayerSpace;

public class SkillJoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform lever;
    private RectTransform rectTransform;

    public Player player;
    [SerializeField, Range(10f, 150f)] private float leverRange;
    [SerializeField] Image[] alphaImage = new Image[3];
    [SerializeField] Image skillImg;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        AlphaJoyStick();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        var realpos = new Vector2(Screen.width - rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

        var inputDir = eventData.position - new Vector2(rectTransform.position.x, rectTransform.position.y);

        var clampedDir = inputDir.magnitude < leverRange ?
            inputDir : inputDir.normalized * leverRange;

        lever.anchoredPosition = clampedDir;
        SetDirection();

    }

    public void OnDrag(PointerEventData eventData)
    {
        var realpos = new Vector2(Screen.width - rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

        var inputDir = eventData.position - new Vector2(rectTransform.position.x, rectTransform.position.y);
        var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;

        lever.anchoredPosition = clampedDir;
        SetDirection();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;

    }

    public void SetDirection()
    {
    }

    public void AlphaJoyStick()
    {
        for(int i = 0; i < 3; ++i)
        {
            alphaImage[i].color = new Color(alphaImage[i].color.r, alphaImage[i].color.g, alphaImage[i].color.b, 180 / 255f);
        }
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

    public void DisActiveJoyStick()
    {
        gameObject.SetActive(false);
    }
}

