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
            if (!PhotonNetwork.OfflineMode)
                photonView.RPC("RegisterforMasterClient", RpcTarget.All);
            else
                RegisterforMasterClient();
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

        public void CurHpIncrease(string target, float amount)
        {
            dicPlayerInfo.GetValueOrDefault(target).Heal(amount);
        }

        public void CurHpDecrease(GameObject owner, GameObject target, float damage)
        {

        }


        public int CurHpDecrease(string ownerId, string targetId, float damage)
        {
            PlayerInfo target;
            //Damage
            if (DicPlayerInfo.TryGetValue(targetId, out target))
            {
                damage = damage > target.CurHP + target.CurShield ? target.CurHP : damage;
                //Data Regist
                InGamePlayerData data;

                if (IngameDataManager.Instance.Data.TryGetValue(ownerId, out data))
                    data.HitPoint(damage);

                //Ingame 
                damage = DamagedShield(target, damage);
                target.Damaged(damage);
                target.BattleOn();
                return (int)damage;
            }

            return 0;
        }


        public int CurHPDecreaseRatio(string ownerId, string targetId, float ratio)
        {
            PlayerInfo target;
            int damage;
            //Damage
            if (DicPlayerInfo.TryGetValue(targetId, out target))
            {
                damage = (int)(target.MaxHP * ratio);
                damage = (int)(damage > target.CurHP + target.CurShield ? target.CurHP : damage);
                //Ingame 
                ratio = DamagedShield(target, damage);


                //Data Regist
                InGamePlayerData data;

                if (IngameDataManager.Instance.Data.TryGetValue(ownerId, out data))
                    data.HitPoint(damage);

                target.Damaged(damage);
                target.BattleOn();
                return damage;

            }

            return 0;
        }




        public int CurHpDecrease(string targetId, float damage)
        {
            PlayerInfo target;
            //Damage
            if (DicPlayerInfo.TryGetValue(targetId, out target))
            {
                damage = damage > target.CurHP + target.CurShield ? target.CurHP : damage;
                //Ingame 
                damage = DamagedShield(target, damage);

                target.Damaged(damage);
                target.BattleOn();        
                return (int)damage;
            }
            return 0;

        }

        public int CurHPDecreaseRatio(string targetId, float ratio)
        {
            PlayerInfo target;
            //Damage
            if (DicPlayerInfo.TryGetValue(targetId, out target))
            {
                int damage;
                damage = (int)(target.MaxHP * ratio);
                damage = (int)(damage > target.CurHP + target.CurShield ? target.CurHP : damage);
                //Ingame 
                ratio = DamagedShield(target, damage);

                target.Damaged(damage);
                target.BattleOn();
                return damage;

            }
            return 0;

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
                    Debug.Log(killerId);
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

                    if(killerInfo == null)
                    {
                        if(killerId.Equals("MagneticField"))
                        {
                            playerInfoArr[i].PlayerDie(UNITTYPE.Magnetic, "자기장");
                        }
                        else if(killerId.Equals("RedZone"))
                        {
                            playerInfoArr[i].PlayerDie(UNITTYPE.RedZone, "레드존");
                        }
                    }
                }
            }
        }
      
        /// <<<<<<<<<<<<<<<<<<<<<<<<<<
        /// ===========================
        /// Shield Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>

        [PunRPC]
        private float DamagedShield(PlayerInfo target, float damage)
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


        public void GetShield(string target, float amount)
        {
            dicPlayerInfo.GetValueOrDefault(target).GetShield(amount);
        }


        /// <<<<<<<<<<<<<<<<<<<<<<<<<<



        /// ===========================
        /// CurSkillPt Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>

        [PunRPC]
        public void CurSkillPtIncrease(string targetId, float amount)
        {
            PlayerInfo info;

            if (!DicPlayerInfo.TryGetValue(targetId, out info))
                return;

            info.GetSkillPoint(amount);
        }

        [PunRPC]
        public void CurSkillPtDecrease(string targetId, float amount)
        {
            PlayerInfo info;

            if (!DicPlayerInfo.TryGetValue(targetId, out info))
                return;

            info.LoseSkillPoint(amount);
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

        public void DmgUp(string target, float amount)
        {
            DicPlayerInfo.GetValueOrDefault(target).DmgUp(amount);
        }

        public void DmgDown(string target, float amount)
        {
            DicPlayerInfo.GetValueOrDefault(target).DmgDown(amount);
        }

    }
}