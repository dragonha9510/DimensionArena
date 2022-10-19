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
    private TextMeshProUGUI willChangeNameText;
    [SerializeField]
    private GameObject inputField;

    private void OnEnable()
    {
        PhotonNetwork.Disconnect();
    }

    private void Start()
    {
        nickNameText.text = LobbyManagerRenewal.Instance.PlayerName;
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
        }
        else
        {
            // 저장되어있는 DB 정보값 불러와야함
            FirebaseDB_Manager.Instance.ChangeNickName(willChangeNameText.text);
        }
        LobbyManagerRenewal.Instance.ReconnectServerBecauseDB(willChangeNameText.text);
    }

}
