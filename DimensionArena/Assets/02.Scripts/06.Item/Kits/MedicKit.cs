using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;

public class MedicKit : Item
{
    protected override void InteractItem(string targetID)
    {
        Debug.Log("hp Get");
        PlayerInfoManager.Instance.CurHpIncrease(targetID, info.healthAmount);
    }

}
