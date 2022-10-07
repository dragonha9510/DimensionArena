using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


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
    [SerializeField] GameObject[] playerObjectArr;
    [SerializeField] PlayerInfo[] playerInfoArr;
    

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
                    
                    for (int i = 0; i < players.Length; ++i)
                    {
                        playerInfoArr[i] = players[i].GetComponent<Player>().Info;
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
            if (NullCheck.IsNullOrEmpty(playerInfoArr))
            {
                GameObject[] players = PlayerObjectArr;
                playerInfoArr = new PlayerInfo[players.Length];

                for (int i = 0; i < players.Length;++i)
                {
                    playerInfoArr[i] = players[i].GetComponent<Player>().Info;
                }
            }

            return playerInfoArr;
        }
    }

    void Start()
    {
        
    }
    #endregion

    /// <<<<<<<<<<<<<<<<<<<<<<<<<<<
   


    /// ===========================
    /// CurHp Region
    /// >>>>>>>>>>>>>>>>>>>>>>>>>>

    public void AddPlayer()
    {
        photonView.RPC("AddPlayers", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void AddPlayers()
    {
        if(NullCheck.IsNullOrEmpty(playerObjectArr))
        {
            playerObjectArr = GameObject.FindGameObjectsWithTag("Player");
            playerInfoArr = new PlayerInfo[playerObjectArr.Length];
            for (int i = 0; i < playerObjectArr.Length; ++i)
            {
                playerInfoArr[i] = playerObjectArr[i].GetComponent<Player>().Info;
            }
        }
    }


    #region CurHp Method

    [PunRPC]
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

    [PunRPC]
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

    [PunRPC]
    public void CurHpDecrease(GameObject owner, GameObject target, float damage)
    {

        for (int i = 0; i < PlayerObjectArr.Length; ++i)
        {
            if (PlayerObjectArr[i] == target)
            {
                damage = CheckShieldExist(playerInfoArr[i], damage);
                playerInfoArr[i].Damaged(damage);
                break;
            }
        }

        //Do Something wiht Owner relation..
    }


    [PunRPC]
    public void CurHpDecrease(string targetId, float damage)
    {
        for (int i = 0; i < PlayerObjectArr.Length; ++i)
        {
            if (playerInfoArr[i].ID == targetId)
            {            
                playerInfoArr[i].Damaged(damage);
                break;
            }
        }
    }

    [PunRPC]
    public void CurHpDecrease(GameObject owner, string targetId, float damage)
    {
        for (int i = 0; i < PlayerObjectArr.Length; ++i)
        {
            if (playerInfoArr[i].ID == targetId)
            {
                playerInfoArr[i].Damaged(damage);
                //playerInfoArr[i].IsAlive = playerInfoArr[i].CurHP == 0 ? true : false;
                break;
            }
        }

    }

    #endregion

    /// <<<<<<<<<<<<<<<<<<<<<<<<<<


    /// ===========================
    /// Shield Region
    /// >>>>>>>>>>>>>>>>>>>>>>>>>>

    #region Shield Method



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

    #endregion

    /// <<<<<<<<<<<<<<<<<<<<<<<<<<

    /// ===========================
    /// CurSkillPt Region
    /// >>>>>>>>>>>>>>>>>>>>>>>>>>

    #region CurSkillPt Method

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

    #endregion

    /// <<<<<<<<<<<<<<<<<<<<<<<<<<

    /// ===========================
    /// Speed Region
    /// >>>>>>>>>>>>>>>>>>>>>>>>>>
  
    #region Speed Method
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
    #endregion

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
