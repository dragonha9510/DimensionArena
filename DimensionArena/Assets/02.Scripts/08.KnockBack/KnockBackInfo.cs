using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct KnockBackInfo
{
    [HideInInspector] public bool isOn;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float damage;
    [HideInInspector] public float ultimatePoint;
    [HideInInspector] public string ownerID;

    public float speed;
    public float distance;
    public bool isDamage;
    public bool isPercentDamage;
    public bool isEnvironment;
}
