using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagazineUI : MonoBehaviour
{
    Player_Atk ownerAtk;

    void Start()
    {
        ownerAtk = GetComponentInParent<Player_Atk>();
        InsertMagazineUI(ownerAtk.MaxMagazine);
        ownerAtk.eReloading += FillReloadImage;
        ownerAtk.eShotMagazine += UseMagazine;
    }

    void InsertMagazineUI(int cnt)
    {
        for(int i = 0; i < cnt; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void FillReloadImage(int curMagazine, float fillAmount)
    {
        transform.GetChild(curMagazine).
            GetChild(0).GetComponent<Image>().fillAmount = fillAmount;

        if(fillAmount.Equals(1.0f))
        {
            //향후, 추가
        }
    }

    private void UseMagazine(int idx)
    {
        transform.GetChild(idx).
            GetChild(0).GetComponent<Image>().fillAmount = 0.0f;
    }
}
