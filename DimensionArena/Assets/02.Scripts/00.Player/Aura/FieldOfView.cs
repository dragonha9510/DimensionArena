using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Range(0,360)]
    public float viewAngle;
    public float ViewAngle { get { return viewAngle; } }
    [SerializeField]
    private float viewRadius;
    public float ViewRadius { get { return viewRadius; } }
    public Vector3 DirFromAngle(float angleInDegrees)
    {

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    
}
