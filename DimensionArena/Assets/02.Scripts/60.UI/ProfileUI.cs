using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ProfileUI : MonoBehaviourPun
{
    [SerializeField]
    private TextMeshProUGUI nickNameText;
    [SerializeField]
    private TMP_InputField willChangeNameText;
    [SerializeField]
    private GameObject inputField;
    [SerializeField]
    private GameObject failedInformation;

    [SerializeField]
    private TextMeshProUGUI survivalKillCount;
    [SerializeField]
    private TextMeshProUGUI survivalDamage;


    private void OnEnable()
    {
        nickNameText.text = PhotonNetwork.NickName;
        PlayerData data = FirebaseDB_Manager.Instance.GetMyData();
        survivalKillCount.text = data.killCount.ToString();
        survivalDamage.text = data.totalDamage[0].ToString();

    }
    public void Profile_UI_Off()
    {
        this.gameObject.SetActive(false);
    }

    public void NickNameInputFieldOn()
    {
        inputField.SetActive(true);
    }
    public void ChangeNickName()
    {
        inputField.SetActive(false);
        if (FirebaseDB_Manager.Instance.NameOverlapCheck(willChangeNameText.text))
        {
            // 변경될 값이 중복됨
            inputField.SetActive(false);
            StartCoroutine("FailedNotice");
        }
        else
        {
            // 저장되어있는 DB 정보값 불러와야함
            FirebaseDB_Manager.Instance.ReWriteData_Name(willChangeNameText.text);
            PhotonNetwork.NickName = willChangeNameText.text;
            //LobbyManagerRenewal.Instance.ChangeNickNmae(willChangeNameText.text);
            nickNameText.text = willChangeNameText.text;
        }
    }

    IEnumerator FailedNotice()
    {
        failedInformation.SetActive(true);
        while(true)
        {
            yield return new WaitForSeconds(2.0f);
            failedInformation.SetActive(false);
        }
    }

}
