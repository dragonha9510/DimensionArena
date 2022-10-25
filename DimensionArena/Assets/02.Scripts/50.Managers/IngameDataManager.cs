using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PlayerSpace;

public class IngameDataManager : MonoBehaviour
{
    private static IngameDataManager instance;

    public static IngameDataManager Instance
    {
        get
        {
            if(!instance)
            {
                if(!(instance = GameObject.FindObjectOfType<IngameDataManager>()))
                {
                    GameObject obj = new GameObject("IngameDataManager");
                    instance = obj.AddComponent<IngameDataManager>();
                }
            }

            return instance;
        }
    }


  
    Player player;
    InGamePlayerData data;

    private void Awake()
    {
        if (!instance)
            Destroy(this.gameObject);

        FindMyPlayer();
    }

    private void FindMyPlayer()
    {
        GameObject[] players = PlayerInfoManager.Instance.PlayerObjectArr;

        for(int i = 0; i < players.Length; ++i)
        {
            //when this obj is mine
            if(players[i].GetComponent<PhotonView>().IsMine)
            {
                //player regist and break
                player = players[i].GetComponent<Player>();
                break;
            }
        }
    }




}
