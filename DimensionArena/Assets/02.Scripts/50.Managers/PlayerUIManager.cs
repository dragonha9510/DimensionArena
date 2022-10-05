using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUIManager : MonoBehaviour
{
    Player target;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] Image hpBarImage;

    void Start()
    {
        //�÷��̾� �޾ƿ���.
        target = GetComponentInParent<Player>();

        //ȯ�� ���� �б���
        string nickName;
        nickName = 
            target.photonView.Owner == null ? target.photonView.name : target.photonView.Owner.NickName;

        //�̸� ����
        playerName.text = nickName;

        //Target
        target.Info.EcurHPChanged += HpBarChange;
    }



    void HpBarChange(float amount)
    {
        hpBarImage.fillAmount = amount;
    }



    private void OnDestroy()
    {
        target.Info.EcurHPChanged -= HpBarChange;
    }
}