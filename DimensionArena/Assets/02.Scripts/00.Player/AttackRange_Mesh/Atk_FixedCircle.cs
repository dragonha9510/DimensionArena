using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk_FixedCircle : Atk_Range
{
    public override void Calculate_Range(float maxdistance, Vector3 direction)
    {
        if (Mathf.Approximately(direction.magnitude, 0))
            return;

        
        transform.position = owner.position + Vector3.up * 0.01f;
    }
}
