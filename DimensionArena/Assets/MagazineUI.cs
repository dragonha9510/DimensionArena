using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PlayerSpace;

public class MagazineUI : MonoBehaviour
{
    Player_Atk ownerAtk;
    [SerializeField] GameObject parentMagazineUI;
    [SerializeField] Image magazineBar;
    [SerializeField] GameObject prefab_Mask;
    Vector3 originRectPos;
    void Start()
    {
        originRectPos = magazineBar.rectTransform.anchoredPosition;
        Player player = GetComponent<Player>();
        ownerAtk = GetComponentInParent<Player_Atk>();

        if(player)
        {
            if (!player.photonView.IsMine)
            {
                parentMagazineUI.SetActive(false);
                return;
            }
        }

        InsertMagazineUI(ownerAtk.AtkInfo.MaxMagazine);
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
            uiMaskObj.transform.SetParent(this.gameObject.transform);

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
        maskArea = Mathf.Floor(maskArea * 100f) * 0.01f;

        for (int i = 0; i < childIdx; ++i)
            transform.GetChild(i).gameObject.SetActive((magazineBar.fillAmount) >= (maskArea * (i + 1)));

       
    }

    bool isAnimation;
    void OscillateMagazineBar()
    {
        if(!isAnimation)
            StartCoroutine(OscillatetMagazineBarCoroutine());
    }

    private IEnumerator OscillatetMagazineBarCoroutine()
    {
        isAnimation = true;
        magazineBar.rectTransform.DOShakeAnchorPos(0.5f, new Vector2(0.35f, 0.0f));
        yield return new WaitForSeconds(0.5f);
        magazineBar.rectTransform.anchoredPosition = originRectPos;
        isAnimation = false;
    }
}
