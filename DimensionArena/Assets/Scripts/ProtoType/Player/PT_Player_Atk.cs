using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PT_Player_Atk : MonoBehaviourPun
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
        if (photonView.IsMine)
        {
          // GameObject.Find("AtkJoyStick").GetComponent<PT_AtkJoyStick>().player = this;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 forLength = Vector3.zero;
        float distance = range;
        Vector3 position = transform.position + new Vector3(0, 0.5f, 0) + direction.normalized;

        if (Physics.Raycast(position, direction.normalized, out atkRangeRay, range))
        {
            forLength = atkRangeRay.point - position;
            distance = forLength.magnitude;
        }

        atkRangeMesh.transform.forward = (transform.position - atkRangeMesh.transform.position).normalized;
        atkRangeMesh.transform.localScale = new Vector3(0.5f, 1, distance);
        atkRangeMesh.transform.position = transform.position + direction.normalized * ((distance * 0.5f) + 1f) + new Vector3(0, 0.001f, 0);
    }
}
