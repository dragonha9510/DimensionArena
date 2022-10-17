using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Atk_Range : MonoBehaviour
{
    [SerializeField] protected Transform owner;

    public abstract void Calculate_Range(float maxdistance, Vector3 direction);
}
