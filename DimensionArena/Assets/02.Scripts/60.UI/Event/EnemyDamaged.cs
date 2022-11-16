using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamaged : MonoBehaviour
{
    bool isTrigger;
    public bool IsTrigger => isTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackCollider"))
            isTrigger = true;
    }
}
