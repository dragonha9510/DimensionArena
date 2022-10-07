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
    [SerializeField] GameObject arrow;
    [SerializeField] float arrowTime = 3.0f;
    void Start()
    {

        //플레이어 받아오기.
        target = GetComponentInParent<Player>();

        if(target.photonView.IsMine)
        {
            arrow.SetActive(true);
        }
        //환경 정보 분기점
        string nickName;
        nickName = 
            target.photonView.Owner == null ? target.photonView.name : target.photonView.Owner.NickName;

        //이름 지정
        playerName.text = nickName;

        //Target
        target.Info.EcurHPChanged += HpBarChange;

        StartCoroutine(ArrowMoveCoroutine());

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
