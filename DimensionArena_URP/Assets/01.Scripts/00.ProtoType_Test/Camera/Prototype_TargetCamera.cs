using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_TargetCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 interval;

    private void Start()
    {
        interval = transform.position;

        if (target == null)
        {
            GameObject temp = Instantiate(new GameObject(), transform);
            temp.transform.position = Vector3.zero;

            target = temp.transform;
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(target);
        transform.position = target.position + interval;
    }
}
