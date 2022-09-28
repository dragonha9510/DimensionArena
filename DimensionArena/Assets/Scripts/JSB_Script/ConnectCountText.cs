using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class ConnectCountText : MonoBehaviourPun
{
    TextMeshProUGUI countText;
    string defaultText = "Now Connect Count : ";

    private void Start()
    {
        countText = GetComponent<TextMeshProUGUI>();
    }
    public void RefreshServerText(int count)
    {
        photonView.RPC("RefreshConnectCount", RpcTarget.All, count);
    }
    [PunRPC]
    public void RefreshConnectCount(int num)
    {
        countText.text = defaultText + num.ToString();
    }
}
