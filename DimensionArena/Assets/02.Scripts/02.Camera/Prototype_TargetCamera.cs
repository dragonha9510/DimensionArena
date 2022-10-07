using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_TargetCamera : MonoBehaviour
{
    [SerializeField] public Transform target;
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
        //transform.LookAt(target);
        Vector3 location = target.position + interval;
        location.y = transform.position.y;
        Vector3 direction = location - transform.position;

        float speed = direction.magnitude * 2;
        direction.Normalize();

        transform.position = transform.position + (direction * speed * Time.deltaTime);
    }
}
