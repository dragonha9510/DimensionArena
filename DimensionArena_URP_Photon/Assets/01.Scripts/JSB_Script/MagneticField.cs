using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : MonoBehaviour
{
    [SerializeField] GameObject Ground;
    [SerializeField] GameObject magneticRect;
    private Bounds groundBounds;
    void Start()
    {
        groundBounds = Ground.GetComponent<Collider>().bounds;
    }

    void Update()
    {
        Debug.Log("center : " + groundBounds.center);
        Debug.Log("size : " + groundBounds.size);
        Debug.Log("extents : " + groundBounds.extents);

    }
}
