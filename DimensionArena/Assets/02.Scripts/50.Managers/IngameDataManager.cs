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
        death = 0;
        rank = 0;
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

    public void DeathData()
    {
        death++;
        liveTime = IngameDataManager.Instance.CurTime;



        Debug.Log("길이 : " + PlayerInfoManager.Instance.PlayerObjectArr.Length);

        for(int i = 0; i < PlayerInfoManager.Instance.PlayerObjectArr.Length; ++i)
        {
            Debug.Log("rank : " + rank);
            if(PlayerInfoManager.Instance.PlayerObjectArr[i].activeInHierarchy)
                rank++;
        }     


        //임시함수 제거해야된다
        if(rank == 2)
        {
            for (int i = 0; i < PlayerInfoManager.Instance.PlayerObjectArr.Length; ++i)
            {
                if (PlayerInfoManager.Instance.PlayerObjectArr[i].activeInHierarchy)
                {
                    Debug.Log("임시함수들어감");
                    PlayerInfoManager.Instance.PlayerObjectArr[i].GetComponent<Player>().Win();
                }

            }
        }
    }


    public void SetRank()
    {
        rank = 1;
    }

    float damage;
    public float Damage;

    float liveTime;
    public float LiveTime => liveTime;
    int kill;
    public int Kill => kill;
    int death;
    public int Death => death;

    int rank;
    public int Rank => rank;
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

    private float time = 0.0f;
    public float CurTime => time;

    private InGamePlayerData ownerData;
    public InGamePlayerData OwnerData => ownerData;

    private void Awake()
    {     
        if(!Instance)
        {
            Destroy(this.gameObject);
        }

        FindAllPlayer();
    }


    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;


        time += Time.deltaTime;

    }


    private void FindAllPlayer()
    {
        data = new Dictionary<string, InGamePlayerData>();

        GameObject[] players = PlayerInfoManager.Instance.PlayerObjectArr;

        for(int i = 0; i < players.Length; ++i)
        {
            data.Add(players[i].name, new InGamePlayerData(players[i].name));

            if(ownerData == null)
            {
                GameObject obj;
                PlayerInfoManager.Instance.DicPlayer.TryGetValue(players[i].name, out obj);

                if (obj.GetComponent<PhotonView>().IsMine)
                    ownerData = data[players[i].name];
            }

        }
    }

    public void DestroyManager()
    {
        Destroy(this.gameObject);
    }
}
