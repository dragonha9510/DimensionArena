using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerSpace;


public class PlayerUIManager : MonoBehaviour
{
    Player target;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] Slider hpBarSlider;
    [SerializeField] GameObject arrow;
    [SerializeField] float arrowTime = 3.0f;
    void Start()
    {

        //�÷��̾� �޾ƿ���.
        target = GetComponentInParent<Player>();



        // JSB -> ARROW ����
        if(arrow == null)
        {
            playerName.text = "Player";
        }
        else if(target.photonView.IsMine)
        {
            arrow.SetActive(true);
            //ȯ�� ���� �б���
            string nickName;
            nickName =
                target.photonView.Owner == null ? target.photonView.name : target.photonView.Owner.NickName;


            //�̸� ����
            playerName.text = nickName;
            StartCoroutine(ArrowMoveCoroutine());
        }
        //////////////




        //Target
        target.Info.EcurHPChanged += HpBarChange;


    }

    IEnumerator ArrowMoveCoroutine()
    {    
        //5�� ��鸲
        for(int i = 0; i < 5; ++i)
        {
            //��
            for (int j = 0; j < 20; ++j)
            {
                arrow.transform.position += (Vector3.up * 0.025f);
                yield return new WaitForSeconds(arrowTime * 0.005f);
            }

            //��
            for (int j = 0; j < 20; ++j)
            {
                arrow.transform.position += (Vector3.down * 0.025f);
                yield return new WaitForSeconds(arrowTime * 0.005f);
            }
        }
        arrow.SetActive(false);
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
