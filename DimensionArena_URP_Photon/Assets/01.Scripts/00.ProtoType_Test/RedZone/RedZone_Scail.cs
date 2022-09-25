using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class RedZone_Scail : MonoBehaviour
{
    [Tooltip("RectangleBoundary ( innerCircle )")]
    [SerializeField] private Vector2 size = new Vector2(8, 8);
    [Space (10f)]
    [Tooltip("Input Decal Object")]
    [SerializeField] private DecalProjector proj;
    

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(size.x, 1, size.y);
        proj.size = new Vector3(size.x, size.y, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            transform.localScale = new Vector3(size.x, 1, size.y);
        }
    }
}
