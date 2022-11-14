using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk_Circle : Atk_Range
{
    [SerializeField] private float yPosInterval = 0.01f;

    public override void Calculate_Range(float maxdistance, Vector3 direction)
    {
        if (Mathf.Approximately(direction.magnitude, 0))
            return;

        transform.position = owner.position + direction * maxdistance + Vector3.up * yPosInterval;
    }
}
