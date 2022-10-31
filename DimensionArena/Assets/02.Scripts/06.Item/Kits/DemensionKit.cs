using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;

public class DemensionKit : Item
{
    protected override void InteractItem(string targetID)
    {
        Debug.Log("All Get");

        PlayerInfoManager.Instance.DmgUp(targetID, info.attackIncrement);
        PlayerInfoManager.Instance.SpeedIncrease(targetID, info.speedAmount);
        PlayerInfoManager.Instance.CurHpIncrease(targetID, info.healthAmount);


    }

}
