using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
        speed = 3.0f;
        maxSpeed = 6.0f;
        curShield = 0.0f;
        maxShield = 100.0f;
        isAlive = true;
    }

    /// =============================
    /// Action Region
    /// =============================
    public event Action<float> EskillAmountChanged = (param) => { };
    public event Action<float> EcurHPChanged = (param) => { };
    public event Action<bool> EDeadPlayer = (param) => { };


    /// =============================
    /// Player Information Region
    /// =============================
    #region Player Information

    [SerializeField] private string id;
    [SerializeField] private float maxHP;
    [SerializeField] private float curHP;
    [SerializeField] private float curSkillPoint;
    [SerializeField] private float maxSkillPoint;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float maxSpeed = 6.0f;
    [SerializeField] private bool  isAlive;

    [SerializeField] private float curShield;
    [SerializeField] private float maxShield;

    public float MaxHP { get { return maxHP; }  }
    public float CurHP { get { return CurHP; } }
    public float CurSkillPoint { get { return curSkillPoint; } }
    public float MaxSkillPoint { get { return maxSkillPoint; } }
    public float Speed { get { return speed; } }
    public float MaxSpeed { get { return maxSpeed; } }
    public float CurShield { get { return curShield; } }
    public float MaxShield { get { return maxShield; } }
    public string ID { get { return id; } }
    public bool IsAlive { get { return isAlive; } private set { isAlive = value; EDeadPlayer(isAlive); }  }
    #endregion



    #region PlayerInfo Related Method

    [PunRPC]
    public void LoseSkillPoint(float point)
    {        
        if (curSkillPoint != maxSkillPoint)
        {
            curSkillPoint -= point;
            curSkillPoint = Mathf.Max(curSkillPoint, 0);
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
            EskillAmountChanged(curSkillPoint / maxSkillPoint);
        }
    }

    [PunRPC]
    public void Damaged(float damage)
    {
        curHP -= damage;
        curHP = Mathf.Max(curHP, 0);
        EcurHPChanged(curHP / maxHP);
    }

    [PunRPC]
    public void Heal(float amount)
    {
        curHP += amount;
        curHP = Mathf.Max(curHP, MaxHP);
        EcurHPChanged(curHP/maxHP);
    }

    [PunRPC]
    public void GetShield(float amount)
    {
        curShield += amount;
        curShield = Mathf.Min(curShield, maxShield);
    }

    [PunRPC]
    public void DamageShield(float amount)
    {
        curShield -= amount;
        curShield = Mathf.Max(curShield, 0);
    }

    [PunRPC]
    public void SpeedUp(float amount)
    {
        speed += amount;
        speed = Mathf.Min(speed, maxSpeed);
    }

    [PunRPC]
    public void SpeedDown(float amount)
    {
        speed -= amount;
        speed = Mathf.Max(speed, 1);
    }

    #endregion

}
