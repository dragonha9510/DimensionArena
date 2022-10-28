using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct KnockBackInfo
{
    [HideInInspector] public bool isOn;
    [HideInInspector] public Vector3 direction;

    public float speed;
    public float distance;
}
