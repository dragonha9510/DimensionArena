using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RedZone_Missile_Item : RedZone_Missile
{
    [SerializeField] private GameObject itemBox;
    private bool isCreate;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.name != gameObject.name && !isCreate)
        {
            base.OnTriggerEnter(other);
            Instantiate(itemBox, 
                new Vector3((float)((int)transform.position.x + 0.5f), 
                0.5f,
                (float)((int)transform.position.z + 0.5f)), 
                itemBox.transform.rotation);

            isCreate = true;
        }
    }
}
