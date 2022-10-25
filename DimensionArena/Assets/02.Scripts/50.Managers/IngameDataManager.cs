using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PlayerSpace;

public class InGamePlayerData
{
    public InGamePlayerData(string ownerID)
    {
        damage = 0;
        liveTime = 0;
        kill = 0;

        GameObject obj;
        PlayerInfoManager.Instance.DicPlayer.TryGetValue(ownerID, out obj);

        if (obj.GetComponent<PhotonView>().IsMine)
            isMine = true;
        else
            isMine = false;
    }

    public void HitPoint(float dmg)
    {
        damage += dmg;
        Debug.Log(damage);
    }

    public void KillPoint()
    {
        kill++;
        Debug.Log(kill);
    }

    float damage;
    float liveTime;
    int kill;
    bool isMine;
}


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


    [SerializeField] private Dictionary<string, InGamePlayerData> data;
    public  Dictionary<string, InGamePlayerData> Data => data;

    private void Awake()
    {
        if (!instance)
            Destroy(this.gameObject);

        FindAllPlayer();
    }

    private void FindAllPlayer()
    {
        data = new Dictionary<string, InGamePlayerData>();

        GameObject[] players = PlayerInfoManager.Instance.PlayerObjectArr;

        for(int i = 0; i < players.Length; ++i)
        {
            data.Add(players[i].name, new InGamePlayerData(players[i].name));
        }
    }
}
