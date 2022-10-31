using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;

public class SpeedKit : Item
{
    protected override void InteractItem(string targetID)
    {
        Debug.Log("speed Get");
        PlayerInfoManager.Instance.SpeedIncrease(targetID, info.speedAmount);

    }
}
