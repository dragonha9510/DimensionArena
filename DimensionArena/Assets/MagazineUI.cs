using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MagazineUI : MonoBehaviour
{
    Player_Atk ownerAtk;
    [SerializeField] GameObject parentMagazineUI;
    [SerializeField] Image magazineBar;
    [SerializeField] GameObject prefab_Mask;

    void Start()
    {
        ownerAtk = GetComponentInParent<Player_Atk>();
        if(ownerAtk.Owner)
        {
            if (!ownerAtk.Owner.photonView.IsMine)
            {
                parentMagazineUI.SetActive(false);
                return;
            }
        }


        InsertMagazineUI(ownerAtk.MaxMagazine);
        ownerAtk.eChangeMagazineCost += SetMagazineUI;
        ownerAtk.eCantAttack += OscillateMagazineBar;

    }

    
    void InsertMagazineUI(int cnt)
    {
        Image ParentImg = gameObject.GetComponent<Image>();

        int middleIdx = cnt / 2;
        float offset = 0.0f;
        bool isOdd = cnt % 2 == 0 ? false : true;

        if (!isOdd)
            offset = (ParentImg.rectTransform.sizeDelta.x / cnt) * 0.5f;

        for (int i = 0; i < cnt; ++i)
        {
            int offsetIdx = i - middleIdx;
            GameObject uiMaskObj = Instantiate(prefab_Mask);
            uiMaskObj.transform.parent = this.gameObject.transform;

            Image uiMask = uiMaskObj.GetComponent<Image>();
            uiMask.rectTransform.localScale = ParentImg.rectTransform.localScale;
            uiMask.rectTransform.rotation = ParentImg.rectTransform.rotation;
            uiMask.rectTransform.sizeDelta = new Vector2(ParentImg.rectTransform.sizeDelta.x / cnt,
                                                         ParentImg.rectTransform.sizeDelta.y);
            uiMask.rectTransform.anchoredPosition3D = 
                 new Vector3(offsetIdx * uiMask.rectTransform.sizeDelta.x + offset, 0, 0);

            uiMaskObj.SetActive(false);
        }
    }

    void SetMagazineUI(float fillAmount)
    {
        magazineBar.fillAmount = fillAmount;
        int childIdx = transform.childCount;
        float maskArea = (float)(1.0f / childIdx);

        for(int i = 0; i < childIdx; ++i)
        {
            bool isActive = (fillAmount) >= (maskArea * (i + 1));
            transform.GetChild(i).gameObject.SetActive(isActive);
        }
       
    }


    void OscillateMagazineBar()
    {
        magazineBar.rectTransform.DOShakeAnchorPos(0.5f, new Vector2(0.35f, 0.0f));
    }
}