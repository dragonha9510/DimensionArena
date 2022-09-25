using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZone_Missile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name != gameObject.name)
            Destroy(this.gameObject);
    }
}
