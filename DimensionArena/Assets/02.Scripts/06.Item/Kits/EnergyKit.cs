using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;
public class EnergyKit : Item
{
    protected override void InteractItem(string targetID)
    {
        Debug.Log("energy Get");
        PlayerInfoManager.Instance.CurSkillPtIncrease(targetID, info.skillRecovery);
    }
}
