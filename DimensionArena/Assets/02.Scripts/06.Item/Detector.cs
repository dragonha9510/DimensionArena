
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField]
    float followSpeed = 1.0f;

    private bool follow;
    private Vector3 followTarget;

    public Item item;

    private void FixedUpdate()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (follow)
        {
            Vector3 direction = followTarget - this.transform.position;
            GetComponentInParent<Item>().Trans.Translate(direction.normalized * followSpeed * Time.deltaTime,Space.World);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!item)
                return;

            if(item.CurTime < 5.0f)
            {
                if (!item.ownerName.Equals(other.gameObject.name))
                    return;
            }

            follow = true;
            followTarget = other.gameObject.transform.position;
        }
    }
}
