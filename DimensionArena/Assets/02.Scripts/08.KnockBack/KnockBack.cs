using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class KnockBack : MonoBehaviour
{
    [HideInInspector] public KnockBackInfo info;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        isKnockBack temp = other.GetComponent<isKnockBack>();

        if (temp == null)
        {
            Destroy(this.gameObject);
            return; 
        }

        if (temp.info.isOn && temp.enabled)
        {
            Destroy(this.gameObject);
            return;
        }

        temp.enabled = true;
        temp.info.isOn = true;

        temp.info.direction = (other.transform.position - transform.position);
        temp.info.direction.y = 0;
        temp.info.direction.Normalize();

        temp.info.speed = info.speed;
        temp.info.distance = info.distance;

        temp.SetValue();

        Destroy(this.gameObject);
    }
}
