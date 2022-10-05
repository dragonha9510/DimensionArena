using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUIManager : MonoBehaviour
{
    Player target;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] Slider hpBarSlider;

    void Start()
    {
        //플레이어 받아오기.
        target = GetComponentInParent<Player>();

        //환경 정보 분기점
        string nickName;
        nickName = 
            target.photonView.Owner == null ? target.photonView.name : target.photonView.Owner.NickName;

        //이름 지정
        playerName.text = nickName;

        //Target
        target.Info.EcurHPChanged += HpBarChange;
    }



    void HpBarChange(float amount)
    {
        hpBarSlider.value = amount;
    }



    private void OnDestroy()
    {
        target.Info.EcurHPChanged -= HpBarChange;
    }
}
