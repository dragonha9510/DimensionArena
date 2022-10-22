using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
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

        //플레이어 받아오기.
        target = GetComponentInParent<Player>();
        // JSB -> ARROW 설정
        if(!PhotonNetwork.IsConnected)
            playerName.text = "Player"; 
        
        //target name 
        playerName.text = target.photonView.Owner.NickName;

        //Target hp
        target.Info.EcurHPChanged += HpBarChange;

        if(target.photonView.IsMine)
        {
            arrow.SetActive(true);
            StartCoroutine(ArrowMoveCoroutine());
            //이름 지정
        }

    }

    IEnumerator ArrowMoveCoroutine()
    {    
        //5번 흔들림
        for(int i = 0; i < 5; ++i)
        {
            //상
            for (int j = 0; j < 20; ++j)
            {
                arrow.transform.position += (Vector3.up * 0.025f);
                yield return new WaitForSeconds(arrowTime * 0.005f);
            }

            //하
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
