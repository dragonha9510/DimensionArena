using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DitectionRange : MonoBehaviour
{
    [Tooltip("min : x, max : y")]
    [SerializeField] private Vector2 ditectionRange;
    private SphereCollider sphereCollider;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void SetRangeMax()
    {
        if (sphereCollider == null)
            return;

        sphereCollider.radius = ditectionRange.y;
    }

    public void SetRangeMin()
    {
        if (sphereCollider == null)
            return;

        sphereCollider.radius = ditectionRange.x;
    }
}
