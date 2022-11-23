using System;
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

            foreach (GameObject obj in dicPlayer.Values)
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
        [Serializable]
        public class StringPlayerInfoDictionary
            : SerializableDictionary<string, PlayerInfo> { }

        [Serializable]
        public class StringGameobjectDictionary
            : SerializableDictionary<string, GameObject> { }

        [SerializeField] private StringPlayerInfoDictionary dicPlayerInfo;
        [SerializeField] private StringGameobjectDictionary dicPlayer;

        int length;
        public int SurvivalCount => length;
        private Queue<Buf> bufs = new Queue<Buf>();
       
        public StringPlayerInfoDictionary DicPlayerInfo => dicPlayerInfo;
        public StringGameobjectDictionary DicPlayer => dicPlayer;

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
            dicPlayerInfo = new StringPlayerInfoDictionary();
            dicPlayer = new StringGameobjectDictionary();
            length = players.Length;

            for (int i = 0; i < players.Length; ++i)
            {
                //리스트 등록, 딕셔너리 등록
                dicPlayerInfo.Add(new SerializableDictionary<string, PlayerInfo>.Pair(players[i].name, players[i].GetComponent<Player>().Info));
                DicPlayer.Add(new SerializableDictionary<string, GameObject>.Pair(players[i].name, players[i]));
            }

        }
        /// <<<<<<<<<<<<<<<<<<<<<<<<<<<



        /// ===========================
        /// CurHp Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>


        public void CurHpIncrease(string target, float amount)
        {
            PlayerInfo temp;
            if(dicPlayerInfo.TryGetValue(target, out temp))
            {
                temp.Heal(amount);
            }
        }

        // Only CallMaster
        public int CurHpDecreaseAllClient(string killerId,string targetId,float damage)
        {
            photonView.RPC(nameof(CurHpDecrease), RpcTarget.Others, killerId, targetId, damage);
            return CurHpDecrease(killerId, targetId, damage);
        }

        [PunRPC]
        public int CurHpDecrease(string killerId, string targetId, float damage)
        {
            PlayerInfo target;
            //Damage
            if (DicPlayerInfo.TryGetValue(targetId, out target))
            {
                //Damage Calculate
                damage = damage > target.CurHP + target.CurShield ? target.CurHP : damage;
                damage = DamagedShield(target, damage);

                target.Damaged(damage);
                target.BattleOn();

                //Data Save
                InGamePlayerData data;

                if (IngameDataManager.Instance.Data.TryGetValue(killerId, out data))
                    data.HitPoint(damage);

                //Dead Check
                if (target.CurHP <= 0)
                    DeadCheckCallServer(killerId, targetId);

                return (int)damage;
            }

            return 0;
        }


        // Only CallMaster
        public int CurHPDecreaseRatioAllClient(string killerId,string targetId, float ratio)
        {
            photonView.RPC(nameof(CurHPDecreaseRatio), RpcTarget.Others, killerId, targetId, ratio);
            return CurHPDecreaseRatio(killerId, targetId, ratio);
        }

        [PunRPC]
        public int CurHPDecreaseRatio(string killerId, string targetId, float ratio)
        {
            PlayerInfo target;
            int damage;
            //Damage
            if (DicPlayerInfo.TryGetValue(targetId, out target))
            {
                //Damage Calculate
                damage = (int)(target.MaxHP * ratio);
                damage = (int)(damage > target.CurHP + target.CurShield ? target.CurHP + target.CurShield : damage);
                damage = DamagedShield(target, damage);
                target.Damaged(damage);
                target.BattleOn();

                //Data Regist
                InGamePlayerData data;

                if (IngameDataManager.Instance.Data.TryGetValue(killerId, out data))
                    data.HitPoint(damage);

                //Dead Check
                if(target.CurHP <= 0)
                    DeadCheckCallServer(killerId, targetId);

                return damage;

            }

            return 0;
        }


        //JSB
        public void DeadCheckCallServer(string killerId, string targetId)
        {
            if(PhotonNetwork.InRoom)
                photonView.RPC(nameof(HealthCheck), RpcTarget.All, killerId, targetId);
            else
            {
                if (dicPlayerInfo[targetId].CurHP <= 0)
                    dicPlayer[targetId].GetComponent<Player>().DisActiveAnimation();
            }
        }

        [PunRPC]
        private void HealthCheck(string killerId, string targetId)
        {
            GameObject target;
            if (!DicPlayer.TryGetValue(targetId, out target))
                return;

            PlayerInfo targetInfo;
                if (!DicPlayerInfo.TryGetValue(targetId, out targetInfo))
                    return;

            PlayerInfo killerInfo;
                DicPlayerInfo.TryGetValue(killerId, out killerInfo);


            if(target.activeInHierarchy && targetInfo.CurHP <= 0)
            {
                //GameData Set
                InGamePlayerData data;

                if (IngameDataManager.Instance.Data.TryGetValue(targetId, out data))
                    data.ResultData(true);


                if (IngameDataManager.Instance.Data.TryGetValue(killerId, out data))
                    data.KillPoint();

                //환경에 의해 사망했을 경우
                if(null == killerInfo)
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
        // string 을 통한 호출을 위한 오버로딩
        [PunRPC]
        private int DamagedShield(string target, float damage)
        {
            PlayerInfo temp;
            if (dicPlayerInfo.TryGetValue(target, out temp))
            {
                if (temp.CurShield > 0)
                {
                        temp.DamageShield(damage);
                    damage -= temp.CurShield;
                    damage = Mathf.Max(damage, 0);
                }
            }
            //Shield가 존재한다면 Shield를 깍고 남은 데미지 주기
            return (int)damage;
        }
        public void GetShield(string target, float amount)
        {
            PlayerInfo temp;
            if(dicPlayerInfo.TryGetValue(target, out temp))
                if(temp.IsAlive)
                    temp.GetShield(amount);
        }

        // 아이템을 위한 오버로딩
        public void GetShield(string target, float amount, float durationtime)
        {
            PlayerInfo temp;

            if (dicPlayerInfo.TryGetValue(target, out temp))
            {
                if (temp.IsAlive)
                    temp.GetShield(amount);        
            }
            else
                return;

            ItemEffectAdd(target,durationtime,amount, ITEM.ITEM_SHIELDKIT);
        }

        /// <<<<<<<<<<<<<<<<<<<<<<<<<<



        /// ===========================
        /// CurSkillPt Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>

        // Only Call MasterClient
        public void CurSkillPtInCreaseAllClient(string targetId,float amount)
        {
            photonView.RPC(nameof(CurSkillPtDecrease), RpcTarget.All, targetId, amount);
        }

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
        [PunRPC]
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
            PlayerInfo temp;

            if (0 == bufs.Count || !PhotonNetwork.IsMasterClient)
                return;

            foreach(Buf inBuf in bufs)
            {          
                inBuf.durationTime -= Time.fixedDeltaTime;
                if (inBuf.durationTime <= 0)
                {
                    switch (inBuf.itemType)
                    {
                        case ITEM.ITEM_SHIELDKIT:
                            if (dicPlayerInfo.TryGetValue(inBuf.playerName, out temp))
                                photonView.RPC(nameof(DamagedShield), RpcTarget.All, inBuf.playerName, inBuf.amount);
                            break;
                        case ITEM.ITEM_SPEEDKIT:
                            if (dicPlayerInfo.TryGetValue(inBuf.playerName, out temp))
                                photonView.RPC(nameof(SpeedDecrease), RpcTarget.All, inBuf.playerName, inBuf.amount);
                            break;
                        case ITEM.ITEM_DEMENSIONKIT:
                            if (dicPlayerInfo.TryGetValue(inBuf.playerName, out temp))
                                photonView.RPC(nameof(SpeedIncrease), RpcTarget.All, inBuf.playerName, inBuf.amount);
                            break;
                    }
                    
                }
            }
            foreach (Buf inBuf in bufs)
            {
                if (inBuf.durationTime < 0)
                {
                    bufs.Dequeue();
                    break;
                }
            }
        }

        /// 향후, 필요시 추가 구역

        public void DmgUp(string target, float amount)
        {
            PlayerInfo temp;
            if (dicPlayerInfo.TryGetValue(target, out temp))
            {
                temp.DmgUp(amount);
            }

        }

        public void DmgDown(string target, float amount)
        {
            PlayerInfo temp;

            if (dicPlayerInfo.TryGetValue(target, out temp))
            {
                temp.DmgDown(amount);
            }
        }
        public void DiePlayer()
        { 
            length--;

            if (length <= 1)
            {
                foreach(var info in dicPlayerInfo.Values)
                {
                    if(info.IsAlive)
                    {
                        GameObject player;
                        if(dicPlayer.TryGetValue(info.ID, out player))
                        {
                            if(player.GetComponent<PhotonView>().IsMine)
                            {
                                IngameDataManager.Instance.Data.GetValueOrDefault(info.ID).ResultData(false);
                                InGameUIManager.Instance.ResutUIOn();
                            }
                        }
                    }
                }
            }
        }


    }
}