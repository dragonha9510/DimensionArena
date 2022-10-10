using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerInfoManager : MonoBehaviourPun
{

    [SerializeField] GameObject ingameUIManager;

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
                GameObject infoMgr = GameObject.Find("PlayerInfoManager");//new GameObject("PlayerInfoManager");

                if(!infoMgr)
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
    /// Add Player to Memory Region
    /// >>>>>>>>>>>>>>>>>>>>>>>>>>

    #region Add Player to Memory Region With Property
    [SerializeField] private GameObject[] playerObjectArr;
    [SerializeField] private PlayerInfo[] playerInfoArr;
    private Dictionary<string, PlayerInfo> DicPlayerInfo;

    public GameObject[] PlayerObjectArr
    {
        get 
        {
            if (NullCheck.IsNullOrEmpty(playerObjectArr))
            {
                playerObjectArr = GameObject.FindGameObjectsWithTag("Player");

                if (NullCheck.IsNullOrEmpty(playerInfoArr))
                {
                    GameObject[] players = PlayerObjectArr;
                    playerInfoArr = new PlayerInfo[players.Length];
                    DicPlayerInfo = new Dictionary<string, PlayerInfo>();

                    for (int i = 0; i < players.Length; ++i)
                    {
                        //리스트 등록, 딕셔너리 등록
                        playerInfoArr[i] = players[i].GetComponent<Player>().Info;
                        DicPlayerInfo.Add(players[i].name, playerInfoArr[i]);
                    }
                }
            }

            return playerObjectArr;
        }
    }



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
        DicPlayerInfo = new Dictionary<string, PlayerInfo>();

        for (int i = 0; i < players.Length; ++i)
        {
            //리스트 등록, 딕셔너리 등록
            playerInfoArr[i] = players[i].GetComponent<Player>().Info;
            DicPlayerInfo.Add(players[i].name, playerInfoArr[i]);
        }     


        if(PhotonNetwork.CurrentRoom.PlayerCount 
            == players.Length)
        {
            ingameUIManager.SetActive(true);
        }
    }

    /// <<<<<<<<<<<<<<<<<<<<<<<<<<<



    /// ===========================
    /// CurHp Region
    /// >>>>>>>>>>>>>>>>>>>>>>>>>>

    public void CurHpIncrease(GameObject owner, GameObject target, float amount)
    {

        for(int i = 0; i < PlayerObjectArr.Length; ++i)
        {
            if(PlayerObjectArr[i] == target)
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
       

        /*
        for (int i = 0; i < PlayerObjectArr.Length; ++i)
        {
            if (PlayerObjectArr[i] == target)
            {
                damage = CheckShieldExist(playerInfoArr[i], damage);
                playerInfoArr[i].Damaged(damage);
                break;
            }
        }
        */
    }


    /*
    [PunRPC]
    public void CurHpDecrease(string targetId, float damage)
    {
        for (int i = 0; i < playerInfoArr.Length; ++i)
        {
            if (playerInfoArr[i].ID == targetId)
            {
                playerInfoArr[i].Damaged(damage);

                if (playerInfoArr[i].CurHP.
                    AlmostEquals(0.0f, float.Epsilon))
                {
                    playerInfoArr[i].PlayerDie();
                }
                break;
            }
        }
    }
    */

    public void CurHpDecrease(string ownerId, string targetId, float damage)
    {
        PlayerInfo target;

        //Damage
        if (DicPlayerInfo.TryGetValue(targetId, out target))
        {
            damage = CheckShieldExist(target, damage);         
            target.Damaged(damage);
        }

        //When Die
        //if (target.CurHP.AlmostEquals(0.0f, float.Epsilon))
        //{
        //    PlayerInfo owner;
        //    if (DicPlayerInfo.TryGetValue(ownerId, out owner))
        //    {
        //
        //        if(photonView.IsMine)
        //            target.PlayerDie(owner.Type, ownerId);
        //    }
        //}
    }
    // JSB

    public void DeadCheckCallServer(string killerId)
    {
        photonView.RPC("HealthCheck", RpcTarget.All , killerId);
    }

    [PunRPC]
    private void HealthCheck(string killerId)
    {        
        for (int i = 0; i < playerInfoArr.Length; ++i)
        { 
            if(playerInfoArr[i].CurHP <= 0 && playerInfoArr[i].IsAlive)
            {
                //Player disactive and DisActive 
                playerObjectArr[i].SetActive(false);
                //Ingame UI Inform Kill
                PlayerInfo killerInfo;
                DicPlayerInfo.TryGetValue(killerId, out killerInfo);
                playerInfoArr[i].PlayerDie(killerInfo.Type, killerId);

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
        if(target.CurShield > 0)
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
