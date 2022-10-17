using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ProfileUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nickNameText;
    [SerializeField]
    private TextMeshProUGUI willChangeNameText;
    [SerializeField]
    private GameObject inputField;
    [SerializeField]
    private GameObject failedInformation;
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
        if (LobbyManagerRenewal.Instance.NameOverLapCheck(willChangeNameText.text))
        {
            // 이름 변경 실패했을때
            failedInformation.SetActive(true);
        }
        nickNameText.text = LobbyManagerRenewal.Instance.PlayerName;
    }

}
