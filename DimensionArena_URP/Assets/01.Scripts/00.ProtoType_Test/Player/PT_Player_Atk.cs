using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT_Player_Atk : MonoBehaviour
{
    [SerializeField] private float range = 3.0f;
    [SerializeField] private GameObject atkRangeMesh;
    [HideInInspector] public Vector3 direction;
    private RaycastHit atkRangeRay;

    // Start is called before the first frame update
    void Start()
    {
        if(atkRangeMesh == null)
            Instantiate(atkRangeMesh, transform);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forLength = Vector3.zero;
        float distance = range;
        Vector3 position = transform.position + new Vector3(0, 0.5f, 0) + direction.normalized;

        if (Physics.Raycast(position, direction.normalized, out atkRangeRay, range))
        {
            forLength = atkRangeRay.point - position;
            distance = forLength.magnitude;
        }

        atkRangeMesh.transform.LookAt(transform.position);
        atkRangeMesh.transform.localScale = new Vector3(0.5f, 1, distance);
        atkRangeMesh.transform.position = transform.position + direction.normalized * ((distance * 0.5f) + 1f) + new Vector3(0, 0.001f, 0);

        //Debug.DrawRay(position, direction.normalized * distance);
    }
}
