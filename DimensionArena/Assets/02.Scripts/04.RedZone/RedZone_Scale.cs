using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class RedZone_Scale : MonoBehaviour
{
    [Tooltip("RectangleBoundary ( innerCircle )")]
    [SerializeField] private Vector2 size = new Vector2(8, 8);

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(size.x, 1, size.y);
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
