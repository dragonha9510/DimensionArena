using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;

public class PowerKit : Item
{
    protected override void InteractItem(string targetID)
    {
        Debug.Log("power Get");
        PlayerInfoManager.Instance.DmgUp(targetID, info.attackIncrement);
    }
}
