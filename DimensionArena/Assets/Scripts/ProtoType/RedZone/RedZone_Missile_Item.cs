using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RedZone_Missile_Item : RedZone_Missile
{
    [SerializeField] private GameObject[] supplyItem;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.name != gameObject.name)
        {
            base.OnTriggerEnter(other);
            GameObject temp = supplyItem[Random.Range(0, supplyItem.Length)];
            Instantiate(temp, new Vector3(transform.position.x, 0, transform.position.z), temp.transform.rotation);
        }
    }
}
