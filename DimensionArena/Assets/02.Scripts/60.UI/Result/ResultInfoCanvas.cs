using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ManagerSpace;

public class ResultInfoCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] TextMeshProUGUI killText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI surviveText;
    
    private void Awake()
    {
        SetUI();
    }

    private void SetUI()
    {
        InGamePlayerData data = IngameDataManager.Instance.OwnerData;

        if(data.Rank == 1)
            rankText.text = data.Rank.ToString() + "st!";
        else if(data.Rank == 2)
            rankText.text = data.Rank.ToString() + "nd!";
        else if (data.Rank == 3)
            rankText.text = data.Rank.ToString() + "rd!";
        else
            rankText.text = data.Rank.ToString() + "th!";


        killText.text = data.Kill.ToString();
        damageText.text = data.Damage.ToString();
        surviveText.text = data.LiveTime.ToString();
    }


    public void ChanageToMainScene()
    {
        PhotonNetwork.LeaveRoom();
        IngameDataManager.Instance.DestroyManager();
        Destroy(IngameDataManager.Instance.gameObject);
        SceneChanger_Loading.Instance.ChangeScene("Lobby_Main");

    }
}
