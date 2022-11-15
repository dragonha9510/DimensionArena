using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk_Rect : Atk_Range
{
    private RaycastHit atkRangeRay;
    [SerializeField] private Transform baseRect;
    [SerializeField] private Transform scaleRect;

    public override void Calculate_Range(float maxdistance, Vector3 direction)
    {
        float distance = maxdistance;
        Vector3 position = owner.position + new Vector3(0, 0.5f, 0);

        int layerMask = 1 << LayerMask.NameToLayer("GroundObject_Brick");

        if (Physics.Raycast(position, direction.normalized, out atkRangeRay, maxdistance, layerMask))
        {
            Vector3 forLength = Vector3.zero;
            forLength = atkRangeRay.point - position;
            distance = forLength.magnitude;
        }

        float yScale = 0.25f * (distance - 1);
        scaleRect.localScale = new Vector3(1, yScale * 2f, 1);
        scaleRect.localPosition = new Vector3(0, 0, yScale * -1f);

        baseRect.localPosition = scaleRect.localPosition + new Vector3(-0.004f, 0, (yScale + 0.25f) * -1f);

        //transform.localScale = new Vector3(transform.localScale.x, 1, distance);

        transform.position = owner.position +
                                          direction.normalized * (distance * 0.001f);
        transform.forward =
                (owner.position - transform.position).normalized;
    }
}
