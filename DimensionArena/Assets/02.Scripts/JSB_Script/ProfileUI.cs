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
            // ����� ���� �ߺ���
        }
        else
        {
            // ����Ǿ��ִ� DB ������ �ҷ��;���
            FirebaseDB_Manager.Instance.ChangeNickName(willChangeNameText.text);
        }
        LobbyManagerRenewal.Instance.ReconnectServerBecauseDB(willChangeNameText.text);
    }

}
