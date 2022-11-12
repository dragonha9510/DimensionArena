using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PlayerSpace;
using ManagerSpace;

namespace ManagerSpace
{

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
        }

        public void KillPoint()
        {
            kill++;
        }

        public void ResultData(bool isDead)
        {
            if(isDead)
                death++;

            rank =  PlayerInfoManager.Instance.SurvivalCount;
            liveTime = IngameDataManager.Instance.CurTime;
            liveTime *= 100;
            liveTime = Mathf.Floor(liveTime) * 0.01f;
        }

      


        float damage;
        public float Damage => damage;

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
                if (!instance)
                {
                    if (!(instance = FindObjectOfType<IngameDataManager>()))
                    {
                        GameObject obj = new GameObject("IngameDataManager");
                        instance = obj.AddComponent<IngameDataManager>();
                    }
                }

                return instance;
            }
        }



        [SerializeField] private Dictionary<string, InGamePlayerData> data;
        public Dictionary<string, InGamePlayerData> Data => data;

        private float time = 0.0f;
        public float CurTime => time;

        private InGamePlayerData ownerData;
        public InGamePlayerData OwnerData => ownerData;

        private void Awake()
        {
            if (instance)
                Destroy(gameObject);
            else
                instance = this;

            DontDestroyOnLoad(gameObject);
            FindAllPlayer();
        }


        private void Update()
        {
            time += Time.deltaTime;
        }


        private void FindAllPlayer()
        {
            data = new Dictionary<string, InGamePlayerData>();

            GameObject[] players = PlayerInfoManager.Instance.DicPlayer.Values.ToArray();

            for (int i = 0; i < players.Length; ++i)
            {
                data.Add(players[i].name, new InGamePlayerData(players[i].name));

                if (ownerData == null)
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
            Destroy(Instance);
            Destroy(gameObject);
        }
    }

}