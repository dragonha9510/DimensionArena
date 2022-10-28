using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PlayerSpace;

namespace ManagerSpace
{
    public enum ENVIROMENT
    {
        REDZONE,
        MAGNETIC
    }



    public class PlayerInfoManager : MonoBehaviourPun
    {

        /// ===========================
        /// Singleton Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>

        #region Singleton Region
        private static PlayerInfoManager instance;

        public static PlayerInfoManager Instance
        {
            get
            {
                if (null == instance)
                {
                    GameObject infoMgr = GameObject.Find("PlayerInfoManager");

                    if (!infoMgr)
                    {
                        infoMgr = new GameObject("PlayerInfoManager");
                        infoMgr.AddComponent<PlayerInfoManager>();
                        infoMgr.AddComponent<PhotonView>();
                    }

                    instance = infoMgr.GetComponent<PlayerInfoManager>();
                }

                return instance;
            }
        }
        #endregion

        /// <<<<<<<<<<<<<<<<<<<<<<<<<<<


        /// ===========================
        /// JSM Written GetTransformFunction With name
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>

        public Transform getPlayerTransform(string name)
        {
            if (playerObjectArr == null)
                return null;

            foreach (GameObject obj in playerObjectArr)
            {
                if (obj.name.Equals(name))
                    return obj.transform;
            }
            return null;
        }

        /// <<<<<<<<<<<<<<<<<<<<<<<<<<<


        /// ===========================
        /// Add Player to Memory Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>

        #region Add Player to Memory Region With Property

        [SerializeField] private GameObject[] playerObjectArr;
        [SerializeField] private PlayerInfo[] playerInfoArr;
        [SerializeField] private Dictionary<string, PlayerInfo> dicPlayerInfo;
        [SerializeField] private Dictionary<string, GameObject> dicPlayer;
        public GameObject[] PlayerObjectArr
        {
            get
            {
                if (NullCheck.IsNullOrEmpty(playerObjectArr))
                {
                    playerObjectArr = GameObject.FindGameObjectsWithTag("Player");

                    if (NullCheck.IsNullOrEmpty(playerInfoArr))
                    {
                        GameObject[] players = playerObjectArr;
                        playerInfoArr = new PlayerInfo[players.Length];
                        dicPlayerInfo = new Dictionary<string, PlayerInfo>();
                        dicPlayer = new Dictionary<string, GameObject>();

                        for (int i = 0; i < players.Length; ++i)
                        {
                            //리스트 등록, 딕셔너리 등록
                            playerInfoArr[i] = players[i].GetComponent<Player>().Info;
                            DicPlayerInfo.Add(players[i].name, playerInfoArr[i]);
                            DicPlayer.Add(players[i].name, players[i]);

                        }
                    }
                }

                return playerObjectArr;
            }
        }


        public Dictionary<string, PlayerInfo> DicPlayerInfo => dicPlayerInfo;
        public Dictionary<string, GameObject> DicPlayer => dicPlayer;



        public PlayerInfo[] PlayerInfoArr
        {
            get
            {
                if (NullCheck.IsNullOrEmpty(playerObjectArr))
                {
                    GameObject[] players = PlayerObjectArr;
                    //생성 위임
                }

                return playerInfoArr;
            }
        }

        #endregion



        /// ===========================
        /// Register Player
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>
        /// 
        public void RegisterPlayer()
        {
            photonView.RPC("RegisterforMasterClient", RpcTarget.All);
        }

        [PunRPC]
        public void RegisterforMasterClient()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            playerInfoArr = new PlayerInfo[players.Length];
            playerObjectArr = new GameObject[players.Length];
            dicPlayerInfo = new Dictionary<string, PlayerInfo>();
            dicPlayer = new Dictionary<string, GameObject>();

            for (int i = 0; i < players.Length; ++i)
            {
                //리스트 등록, 딕셔너리 등록
                playerObjectArr[i] = players[i];
                playerInfoArr[i] = players[i].GetComponent<Player>().Info;
                DicPlayerInfo.Add(players[i].name, playerInfoArr[i]);
                DicPlayer.Add(players[i].name, players[i]);
            }

        }

        /// <<<<<<<<<<<<<<<<<<<<<<<<<<<



        /// ===========================
        /// CurHp Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>

        public void CurHpIncrease(GameObject owner, GameObject target, float amount)
        {

            for (int i = 0; i < PlayerObjectArr.Length; ++i)
            {
                if (PlayerObjectArr[i] == target)
                {
                    //playerInfoArr[i].("Heal", RpcTarget.MasterClient, amount);
                }
            }

            //Do Something wiht Owner relation..
        }

        public void CurHpIncrease(GameObject target, float amount)
        {
            for (int i = 0; i < PlayerObjectArr.Length; ++i)
            {
                if (PlayerObjectArr[i] == target)
                {
                    //playerInfoArr[i].owner.RPC("Heal", RpcTarget.All, amount);
                }
            }
        }

        public void CurHpDecrease(GameObject owner, GameObject target, float damage)
        {

        }


        public void CurHpDecrease(string ownerId, string targetId, float damage)
        {
            PlayerInfo target;
            //Damage
            if (DicPlayerInfo.TryGetValue(targetId, out target))
            {

                damage = damage > target.CurHP ? target.CurHP : damage;

                //Data Regist
                InGamePlayerData data;


                if (IngameDataManager.Instance.Data.TryGetValue(ownerId, out data))
                {
                    Debug.Log(ownerId + "가" + damage + "만큼 데미지 줌");
                    data.HitPoint(damage);
                }

                //Ingame 
                damage = CheckShieldExist(target, damage);

                target.Damaged(damage);
            }
        }



        public void CurHpDecrease(string targetId, float damage)
        {
            PlayerInfo target;
            //Damage
            if (DicPlayerInfo.TryGetValue(targetId, out target))
            {

                damage = damage > target.CurHP ? target.CurHP : damage;

                //Ingame 
                damage = CheckShieldExist(target, damage);

                target.Damaged(damage);
            }
        }

        public void DeadCheckCallServer(ENVIROMENT enviroment)
        {
            photonView.RPC("HealthCheck", RpcTarget.All, enviroment);
        }


        //JSB
        public void DeadCheckCallServer(string killerId)
        {
            photonView.RPC("HealthCheck", RpcTarget.All, killerId);
        }

        [PunRPC]
        private void HealthCheck(string killerId)
        {
            for (int i = 0; i < playerInfoArr.Length; ++i)
            {
                if (playerInfoArr[i].CurHP <= 0 && playerObjectArr[i].activeInHierarchy)
                {
                    //Ingame UI Inform Kill
                    PlayerInfo killerInfo;
                    DicPlayerInfo.TryGetValue(killerId, out killerInfo);
                    playerInfoArr[i].PlayerDie(killerInfo.Type, killerId);

                    //GameData Set
                    InGamePlayerData data;

                    if (IngameDataManager.Instance.Data.TryGetValue(playerInfoArr[i].ID, out data))
                    {
                        data.DeathData();
                    }


                    if (IngameDataManager.Instance.Data.TryGetValue(killerId, out data))
                    {
                        data.KillPoint();
                    }
                    //Player Die 

                }
            }
        }

        [PunRPC]
        private void HealthCheck(ENVIROMENT enviroment)
        {
            for (int i = 0; i < playerInfoArr.Length; ++i)
            {
                if (playerInfoArr[i].CurHP <= 0 && playerObjectArr[i].activeInHierarchy)
                {
                    //Ingame UI Inform Kill
               

                    //GameData Set

                    //Player Die 

                }
            }
        }


        /// <<<<<<<<<<<<<<<<<<<<<<<<<<


        /// ===========================
        /// Shield Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>

        [PunRPC]
        private float CheckShieldExist(PlayerInfo target, float damage)
        {
            //Shield가 존재한다면 Shield를 깍고 남은 데미지 주기
            if (target.CurShield > 0)
            {
                target.DamageShield(damage);
                damage -= target.CurShield;
                damage = Mathf.Max(damage, 0);
            }

            return damage;
        }



        [PunRPC]
        public void ShieldIncrease(string owner, string target, float amount)
        {
            for (int i = 0; i < PlayerObjectArr.Length; ++i)
            {
                if (PlayerObjectArr[i].name == target)
                {
                    //playerInfoArr[i].owner.RPC("GetShield", RpcTarget.All, amount);
                }
            }
            //Do Something wiht Owner relation..


        }
        public void ShieldIncrease(GameObject target, float amount)
        {
            for (int i = 0; i < PlayerObjectArr.Length; ++i)
            {
                if (PlayerObjectArr[i] == target)
                {
                    //playerInfoArr[i].owner.RPC("GetShield", RpcTarget.All, amount);
                }
            }
        }

        /// <<<<<<<<<<<<<<<<<<<<<<<<<<



        /// ===========================
        /// CurSkillPt Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>

        [PunRPC]
        public void CurSkillPtIncrease(string targetId, float amount)
        {
            for (int i = 0; i < PlayerObjectArr.Length; ++i)
            {
                if (playerInfoArr[i].ID == targetId)
                {
                    playerInfoArr[i].GetSkillPoint(amount);
                }
            }
        }

        [PunRPC]
        public void CurSkillPtDecrease(string targetId, float amount)
        {
            for (int i = 0; i < PlayerObjectArr.Length; ++i)
            {
                if (playerInfoArr[i].ID == targetId)
                {
                    playerInfoArr[i].GetSkillPoint(amount);
                }
            }
        }

        /// <<<<<<<<<<<<<<<<<<<<<<<<<<

        /// ===========================
        /// Speed Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>

        public void SpeedIncrease(string owner, string target, float amount)
        {
            for (int i = 0; i < PlayerObjectArr.Length; ++i)
            {
                if (PlayerObjectArr[i].name == target)
                {
                    //playerInfoArr[i].owner.RPC("SpeedUp", RpcTarget.All, amount);
                }
            }
        }
        public void SpeedIncrease(string target, float amount)
        {
            for (int i = 0; i < PlayerObjectArr.Length; ++i)
            {
                if (PlayerObjectArr[i].name == target)
                {
                    //playerInfoArr[i].owner.RPC("SpeedUp", RpcTarget.All, amount);
                }
            }
        }

        public void SpeedDecrease(string owner, string target, float amount)
        {
            for (int i = 0; i < PlayerObjectArr.Length; ++i)
            {
                if (PlayerObjectArr[i].name == target)
                {
                    //playerInfoArr[i].owner.RPC("SpeedDown", RpcTarget.All, amount);
                }
            }
        }
        public void SpeedDecrease(string target, float amount)
        {
            for (int i = 0; i < PlayerObjectArr.Length; ++i)
            {
                if (PlayerObjectArr[i].name == target)
                {
                    //playerInfoArr[i].owner.RPC("SpeedDown", RpcTarget.All, amount);
                }
            }
        }

        /// <<<<<<<<<<<<<<<<<<<<<<<<<<






        /// 향후, 필요시 추가 구역

        public void MaxSkiilPtIncrease(string owner, string target, float damage)
        {

        }
        public void MaxSkiilPtIncrease(string target, float damage)
        {

        }

        public void MaxSkiilPtDecrease(string owner, string target, float damage)
        {

        }
        public void MaxSkiilPtDecrease(string target, float damage)
        {

        }

    }
}