using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class SkillJoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform lever;
    private RectTransform rectTransform;

    Player target;
    [SerializeField, Range(10f, 150f)] private float leverRange;
    [SerializeField] Image[] alphaImage = new Image[3];
    [SerializeField] Image skillImg;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        AlphaJoyStick();
    }

    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            if(player.GetComponent<PhotonView>().IsMine)
            {
                target = player.GetComponent<Player>();
                break;
            }
        }

        target.skillAmountChanged += SkillSetFillAmount;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(skillImg.fillAmount.Equals(1))
        {
            var realpos = new Vector2(Screen.width - rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

            var inputDir = eventData.position - new Vector2(rectTransform.position.x, rectTransform.position.y);

            var clampedDir = inputDir.magnitude < leverRange ?
                inputDir : inputDir.normalized * leverRange;

            lever.anchoredPosition = clampedDir;
            SetDirection();
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skillImg.fillAmount.Equals(1))
        {
            var realpos = new Vector2(Screen.width - rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

            var inputDir = eventData.position - new Vector2(rectTransform.position.x, rectTransform.position.y);
            var clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;

            lever.anchoredPosition = clampedDir;
            SetDirection();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (skillImg.fillAmount.Equals(1))
        {
            lever.anchoredPosition = Vector2.zero;
            SetDirection();
        }
    }

    public void SetDirection()
    {
        //player.direction = new Vector3((lever.position.x - rectTransform.position.x) * 0.01f, 0, (lever.position.y - rectTransform.position.y) * 0.01f);
    }

    public void AlphaJoyStick()
    {
        for(int i = 0; i < 3; ++i)
        {
            alphaImage[i].color = new Color(alphaImage[i].color.r, alphaImage[i].color.g, alphaImage[i].color.b, 180 / 255f);
        }
    }

    public void SkillSetFillAmount(float value)
    {
        skillImg.fillAmount = value;
    }
}

