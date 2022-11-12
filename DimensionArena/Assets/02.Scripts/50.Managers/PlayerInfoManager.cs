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
        int length;
        public int SurvivalCount => length;
        private Queue<Buf> bufs = new Queue<Buf>();


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

            length = PlayerObjectArr.Length;

            for (int i = 0; i < players.Length; ++i)
            {
                //리스트 등록, 딕셔너리 등록
                playerObjectArr[i] = players[i];
                playerInfoArr[i] = players[i].GetComponent<Player>().Info;
                playerInfoArr[i].EDisActivePlayer += DiePlayer;

                DicPlayerInfo.Add(players[i].name, playerInfoArr[i]);
                DicPlayer.Add(players[i].name, players[i]);
            }

        }
        /// <<<<<<<<<<<<<<<<<<<<<<<<<<<



        /// ===========================
        /// CurHp Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>


        public void CurHpIncrease(string target, float amount)
        {
            dicPlayerInfo.GetValueOrDefault(target).Heal(amount);
        }


        public int CurHpDecrease(string killerId, string targetId, float damage)
        {
            PlayerInfo target;
            //Damage
            if (DicPlayerInfo.TryGetValue(targetId, out target))
            {
                damage = damage > target.CurHP + target.CurShield ? target.CurHP : damage;
                //Data Regist
                InGamePlayerData data;

                if (IngameDataManager.Instance.Data.TryGetValue(killerId, out data))
                    data.HitPoint(damage);

                //Ingame 
                damage = DamagedShield(target, damage);
                target.Damaged(damage);
                target.BattleOn();

                if (target.CurHP <= 0)
                    DeadCheckCallServer(killerId, targetId);

                return (int)damage;
            }

            return 0;
        }


        public int CurHPDecreaseRatio(string killerId, string targetId, float ratio)
        {
            PlayerInfo target;
            int damage;
            //Damage
            if (DicPlayerInfo.TryGetValue(targetId, out target))
            {
                damage = (int)(target.MaxHP * ratio);
                damage = (int)(damage > target.CurHP + target.CurShield ? target.CurHP + target.CurShield : damage);
                //Ingame 
                damage = DamagedShield(target, damage);


                //Data Regist
                InGamePlayerData data;

                if (IngameDataManager.Instance.Data.TryGetValue(killerId, out data))
                    data.HitPoint(damage);

                target.Damaged(damage);
                target.BattleOn();

                if(target.CurHP <= 0)
                    DeadCheckCallServer(killerId, targetId);

                return damage;

            }

            return 0;
        }


        //JSB
        public void DeadCheckCallServer(string killerId, string targetId)
        {
            photonView.RPC("HealthCheck", RpcTarget.All, killerId, targetId);
        }

        [PunRPC]
        private void HealthCheck(string killerId, string targetId)
        {
            GameObject target = null;
            int rank = 0;

            Debug.Log(playerObjectArr.Length);

            foreach(var temp in playerObjectArr)
            {
                if(temp.name.Equals(targetId))
                    target = temp;

                if (temp.activeInHierarchy)
                    rank++;
            }
            
            PlayerInfo targetInfo;
            PlayerInfo killerInfo;

            DicPlayerInfo.TryGetValue(targetId, out targetInfo);
            DicPlayerInfo.TryGetValue(killerId, out killerInfo);

            if(target.activeInHierarchy && targetInfo.CurHP <= 0)
            {
                //환경에 의해 사망했을 경우
                if(killerInfo == null)
                {
                    if (killerId.Equals("MagneticField"))
                    {
                        targetInfo.PlayerDie(UNITTYPE.Magnetic, "자기장");
                    }
                    else if (killerId.Equals("RedZone"))
                    {
                        targetInfo.PlayerDie(UNITTYPE.RedZone, "레드존");
                    }
                }
                //플레이어에 의해 사망했을 경우
                else
                {
                    targetInfo.PlayerDie(killerInfo.Type, killerId);                
                }

                //GameData Set
                InGamePlayerData data;

                if (IngameDataManager.Instance.Data.TryGetValue(targetId, out data))
                    data.ResultData(rank, true);


                if (IngameDataManager.Instance.Data.TryGetValue(killerId, out data))
                    data.KillPoint();
            }
        }
      
        /// <<<<<<<<<<<<<<<<<<<<<<<<<<
        /// ===========================
        /// Shield Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>

        [PunRPC]
        private int DamagedShield(PlayerInfo target, float damage)
        {
            //Shield가 존재한다면 Shield를 깍고 남은 데미지 주기
            if (target.CurShield > 0)
            {
                target.DamageShield(damage);
                damage -= target.CurShield;
                damage = Mathf.Max(damage, 0);
            }

            return (int)damage;
        }


        public void GetShield(string target, float amount)
        {
            dicPlayerInfo.GetValueOrDefault(target).GetShield(amount);
        }
        // 아이템을 위한 오버로딩
        public void GetShield(string target, float amount, float durationtime)
        {
            dicPlayerInfo.GetValueOrDefault(target).GetShield(amount);
            ItemEffectAdd(target,durationtime,amount, ITEM.ITEM_SHIELDKIT);
        }

        /// <<<<<<<<<<<<<<<<<<<<<<<<<<



        /// ===========================
        /// CurSkillPt Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>

        [PunRPC]
        public void CurSkillPtIncrease(string targetId, float amount)
        {
            PlayerInfo info;

            if (!DicPlayerInfo.TryGetValue(targetId, out info) || amount.Equals(0))
                return;

            info.GetSkillPoint(amount);
        }

        [PunRPC]
        public void CurSkillPtDecrease(string targetId, float amount)
        {
            PlayerInfo info;

            if (!DicPlayerInfo.TryGetValue(targetId, out info) || amount.Equals(0))
                return;

            info.LoseSkillPoint(amount);
        }

        /// <<<<<<<<<<<<<<<<<<<<<<<<<<

        /// ===========================
        /// Speed Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>

        [PunRPC]
        public void SpeedIncrease(string target, float amount)
        {
            PlayerInfo info;

            if (!DicPlayerInfo.TryGetValue(target, out info))
                return;

            info.SpeedUp(amount);
        }

        // 아이템을 위한 함수
        [PunRPC]
        public void SpeedIncrease(string target, float amount,float durationtime)
        {
            PlayerInfo info;

            if (!DicPlayerInfo.TryGetValue(target, out info))
                return;

            info.SpeedUp(amount);
            ItemEffectAdd(target , durationtime , amount , ITEM.ITEM_SPEEDKIT);
        }

        public void SpeedDecrease(string target, float amount)
        {
            PlayerInfo info;

            if (!DicPlayerInfo.TryGetValue(target, out info))
                return;

            info.SpeedDown(amount);
        }

        /// <<<<<<<<<<<<<<<<<<<<<<<<<<

        public void ItemEffectAdd(string ID,float durationTime,float amount, ITEM itemType)
        {
            bufs.Enqueue(new Buf(ID,durationTime,amount, itemType));
        }

        private void FixedUpdate()
        {
            if (0 == bufs.Count || !PhotonNetwork.IsMasterClient)
                return;
            foreach(Buf inBuf in bufs)
            {
                Debug.Log("아이템 지속시간 감속중.. 아이템 유형 : " + inBuf.itemType);
                inBuf.durationTime -= Time.fixedDeltaTime;
                if (inBuf.durationTime < 0)
                {
                    switch (inBuf.itemType)
                    {
                        case ITEM.ITEM_MEDICKIT:
                            break;
                        case ITEM.ITEM_POWERKIT:
                            break;
                        case ITEM.ITEM_SHIELDKIT:
                            dicPlayerInfo.GetValueOrDefault(inBuf.playerName).DamageShield(inBuf.amount);
                            break;
                        case ITEM.ITEM_SPEEDKIT:
                            dicPlayerInfo.GetValueOrDefault(inBuf.playerName).SpeedDown(inBuf.amount);
                            break;
                        case ITEM.ITEM_ENERGYKIT:
                            break;
                        case ITEM.ITEM_DEMENSIONKIT:
                            dicPlayerInfo.GetValueOrDefault(inBuf.playerName).SpeedDown(inBuf.amount);
                            break;
                        case ITEM.ITEM_END:
                            break;
                    }
                    
                }
            }
            foreach (Buf inBuf in bufs)
            {
                if (inBuf.durationTime < 0)
                {
                    bufs.Dequeue();
                    Debug.Log("아이템 지속시간 종료 효과 제거");
                    break;
                }
               
            }
        }

        /// 향후, 필요시 추가 구역

        public void DmgUp(string target, float amount)
        {
            DicPlayerInfo.GetValueOrDefault(target).DmgUp(amount);
        }

        public void DmgDown(string target, float amount)
        {
            DicPlayerInfo.GetValueOrDefault(target).DmgDown(amount);
        }


        private void DiePlayer()
        {
            length--;

            if (length.Equals(1))
            {
                for (int i = 0; i < playerInfoArr.Length; ++i)
                {
                    if (playerInfoArr[i].CurHP > 0)
                    {
                        IngameDataManager.Instance.Data.GetValueOrDefault(playerObjectArr[i].name).ResultData(1, false);

                        if (playerObjectArr[i].GetComponent<PhotonView>().IsMine)
                        {
                            InGameUIManager.Instance.ResutUIOn();
                        }
                    }
                }

            }
        }



    }
}