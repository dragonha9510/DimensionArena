using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class MatchStateText : MonoBehaviourPun
{
    [SerializeField]
    private TextMeshProUGUI textUI;
    [SerializeField]
    private string watingText = "대기중인 인원 : ";
    [SerializeField]
    private string middleText = " / ";
    [SerializeField]
    private string maxPlayCountText = "최대 인원 : ";


    [PunRPC]
    public void RefreshText(int playerCount, int maxPlayCount)
    {
        textUI.text = watingText + playerCount.ToString()
                        + middleText + maxPlayCountText + maxPlayCount.ToString();
    }

    public void RefreshTextAllClient(int playerCount , int maxPlayCount)
    {
        photonView.RPC("RefreshText", RpcTarget.All,playerCount,maxPlayCount);
    }

}
