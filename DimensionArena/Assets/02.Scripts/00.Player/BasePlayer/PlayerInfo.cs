using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PlayerSpace;
using ManagerSpace;

public enum UNITTYPE
{
    Aura,
    Ravagebell,
    Joohyeok,
    Secilia,
    RedZone,
    Magnetic,
    Securitas
}



[Serializable]
public class PlayerInfo
{
    public PlayerInfo(string ID)
    {
        id = ID;
        maxHP = 100.0f;
        curHP = 100.0f;
        maxSkillPoint = 100.0f;
        curSkillPoint = 0.0f;
        baseSpeed = 3.0f;
        curSpeed = 3.0f;
        curShield = 0.0f;
        isAlive = true;

        additionalDmg = 1.0f;
    }

    public PlayerInfo(string ID, UNITTYPE type,
                      float maxHP, float maxSkillpt,
                      float speed)
    {
        id = ID;
        this.type = type;
        this.maxHP = maxHP;
        curHP = maxHP;
        maxSkillPoint = maxSkillpt;
        curSkillPoint = 0;
        this.baseSpeed = speed;
        this.curSpeed = speed;
        isAlive = true;
        additionalDmg = 1.0f;
    }



    /// =============================
    /// Action Region
    /// =============================
    public event Action<float> EskillAmountChanged = (param) => { };
    public event Action<float> EcurHPChanged = (param) => { };
    public event Action<float> EcurShieldChanged = (param) => { };
    public event Action EDisActivePlayer = () => { };
    public event Action EBattleStateOn = () => { };
    public event Action<string, string> EDeadPlayer = (param, param2) => { };


    /// =============================
    /// Player Information Region
    /// =============================
    #region Player Information

    [Header("Player State")]
    [SerializeField] private string id;
    [SerializeField] private UNITTYPE type;
    [SerializeField] private bool isAlive = true;

    [Header("Player Stat")]
    [SerializeField] private float maxHP;
    [SerializeField] private float curHP;
    [SerializeField] private float curSkillPoint;
    [SerializeField] private float maxSkillPoint;
    [SerializeField] private float curShield;
    [SerializeField] private float maxSheld;
    [SerializeField] private float additionalDmg;
    [SerializeField] private bool isBattle;
    [SerializeField] private float battleOffTime;

    // 속도 증감은 baseSpeed 기준으로 증가한다.
    [SerializeField] private float baseSpeed;
    // 캐릭터의 이동은 curSpeed 기준으로 한다
    [SerializeField] private float curSpeed;


    /// <summary>
    /// If etc variable related with Score So many, then add a new class under the name PlayerScore 
    /// </summary>

    /// <summary>
    /// Etc variable and method Add when Product Design Confirmed...
    /// </summary>

    public float MaxHP { get { return maxHP; } }
    public float CurHP { get { return curHP; } }
    public float CurSkillPoint { get { return curSkillPoint; } }
    public float MaxSkillPoint { get { return maxSkillPoint; } }
    public float Speed { get { return curSpeed; } }
    public float CurShield { get { return curShield; } }
    public float MaxShield { get { return maxSheld; } }
    public float AdditionalDmg { get { return additionalDmg; } }
    public bool IsBattle => isBattle;

    public float BattleOffTime => battleOffTime;
    
    public string ID { get { return id; } set { id = value; } }
    public bool IsAlive { get { return isAlive; } }


    public UNITTYPE Type { get { return type; } }
    #endregion



    #region PlayerInfo Related Method

    [PunRPC]
    public void LoseSkillPoint(float point)
    {
        if (curSkillPoint != maxSkillPoint)
        {
            curSkillPoint -= point;
            curSkillPoint = Mathf.Max(curSkillPoint, 0);
            if (EskillAmountChanged != null)
                EskillAmountChanged(curSkillPoint / maxSkillPoint);
        }
    }

    [PunRPC]
    public void GetSkillPoint(float point)
    {
        if (curSkillPoint != maxSkillPoint)
        {
            curSkillPoint += point;
            curSkillPoint = Mathf.Min(curSkillPoint, maxSkillPoint);
            if(EskillAmountChanged != null)
                EskillAmountChanged(curSkillPoint / maxSkillPoint);
        }
    }

    public void Damaged(float damage)
    {
        curHP -= damage;
        curHP = Mathf.Max(curHP, 0);
        EcurHPChanged(curHP);
    }

    public void Heal(float amount)
    {
        curHP += amount;
        curHP = Mathf.Min(curHP, MaxHP);
        EcurHPChanged(curHP);
    }

    public void GetShield(float amount)
    {
        //Chanage Max Shield
        curShield += amount;
        maxSheld = curShield;
        EcurShieldChanged(curShield);
    }

    public void DamageShield(float amount)
    {
        curShield -= amount;
        curShield = Mathf.Max(curShield, 0);
        EcurShieldChanged(curShield);
    }

    public void DmgUp(float ratio)
    {
        additionalDmg += ratio;
    }

    public void DmgDown(float ratio)
    {
        additionalDmg -= ratio;
    }


    public void SpeedUp(float ratio)
    {
        curSpeed += baseSpeed * ratio;
    }

    public void SpeedDown(float ratio)
    {
        curSpeed -= baseSpeed * ratio;
    }

    public void PlayerDie(string killer_id)
    {
        
        isAlive = false;
        EDeadPlayer(killer_id, id);
        EDisActivePlayer();
    }

    public void PlayerDie()
    {
        isAlive = false;
        EDisActivePlayer();
    }

    public void BattleOn()
    {
        isBattle = true;
        EBattleStateOn();
    }

    public void BattleOff() => isBattle = false;


    #endregion

}
