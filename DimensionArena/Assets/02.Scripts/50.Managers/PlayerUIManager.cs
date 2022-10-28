using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using PlayerSpace;

namespace ManagerSpace
{
    public class PlayerUIManager : MonoBehaviour
    {
        Player target;
        [SerializeField] TextMeshProUGUI playerName;

        [SerializeField] Slider hpBarSlider;
        [SerializeField] TextMeshProUGUI hpText;

        [SerializeField] Image shieldImg;
        [SerializeField] TextMeshProUGUI shieldText;


        [SerializeField] GameObject arrow;
        [SerializeField] float arrowTime = 3.0f;
        void Start()
        {

            //�÷��̾� �޾ƿ���.
            target = GetComponentInParent<Player>();
            // JSB -> ARROW ����
            if (!PhotonNetwork.IsConnected)
                playerName.text = "Player";

            //target name 
            playerName.text = target.photonView.Owner.NickName;
            hpText.text = target.Info.MaxHP.ToString();

            //Target hp
            target.Info.EcurHPChanged += HpChange;

            if (target.photonView.IsMine)
            {
                arrow.SetActive(true);
                StartCoroutine(ArrowMoveCoroutine());
                //�̸� ����
            }

        }

        IEnumerator ArrowMoveCoroutine()
        {
            //5�� ��鸲
            for (int i = 0; i < 5; ++i)
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


        void HpChange(float amount)
        {
            hpBarSlider.value = amount / target.Info.MaxHP;
            hpText.text = amount.ToString();
        }



        void GetShield(float amount)
        {
            //Shield ����
            shieldImg.fillAmount = 1.0f;
            shieldText.text = amount.ToString();
        }

        void ShieldChange(float amount, float ratio)
        {
            shieldText.text = amount.ToString();
            shieldImg.fillAmount = ratio;
        }



        private void OnDestroy()
        {
            target.Info.EcurHPChanged -= HpChange;
        }
    }

}