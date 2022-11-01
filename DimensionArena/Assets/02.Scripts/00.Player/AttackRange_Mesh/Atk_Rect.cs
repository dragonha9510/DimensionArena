using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk_Rect : Atk_Range
{
    private RaycastHit atkRangeRay;

    public override void Calculate_Range(float maxdistance, Vector3 direction)
    {
        float distance = maxdistance;
        Vector3 position = owner.position + new Vector3(0, 0.5f, 0) + direction.normalized;

        if (Physics.Raycast(position, direction.normalized, out atkRangeRay, maxdistance))
        {
            if (atkRangeRay.collider.tag != "Bush" && atkRangeRay.collider.tag != "HideBush" && atkRangeRay.collider.tag != "Water")
            {
                Vector3 forLength = Vector3.zero;
                forLength = atkRangeRay.point - position;
                distance = forLength.magnitude;
            }
        }

        transform.localScale = new Vector3(transform.localScale.x, 1, distance);

        transform.position = owner.position +
                                          direction.normalized * ((distance * 0.5f) + 0.5f)
                                          + new Vector3(0, 0.1f, 0);
        transform.forward =
                (owner.position - transform.position).normalized;
    }
}
