using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField]
    float decreaseRotation = 0.9f;
    [SerializeField]
    float followSpeed = 1.0f;
    Vector3 randBoing;
    private int ColliderCount = 0;

    private bool follow;
    private Vector3 followTarget;

    private void FixedUpdate()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (follow)
        {
            Vector3 direction = followTarget - this.transform.position;
            GetComponentInParent<ItemMove>().Trans.Translate(direction.normalized * followSpeed * Time.deltaTime,Space.World);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Ãæµ¹!!");
            follow = true;
            followTarget = other.gameObject.transform.position;
        }
    }
}
