using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    [Tooltip("min : x, max : y")]
    [SerializeField] private Vector2 detectionRange;
    private SphereCollider sphereCollider;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void SetRangeMax()
    {
        if (sphereCollider == null)
            return;

        sphereCollider.radius = detectionRange.y;
    }

    public void SetRangeMin()
    {
        if (sphereCollider == null)
            return;

        sphereCollider.radius = detectionRange.x;
    }
}
