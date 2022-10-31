using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;

public class ShieldKit : Item
{
    protected override void InteractItem(string targetID)
    {
        Debug.Log("shield Get");
        PlayerInfoManager.Instance.GetShield(targetID, info.shieldAmount);
    }

}
