using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : BaseEvent
{
    [SerializeField] EnemyMove move;
    bool isDetected = false;
    public bool IsDetected => isDetected;
    public override bool CheckEventState()
    {
        return isDetected;
    }

    public override void EventSuccesed()
    {
        if (move)
            move.MoveToZero();
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isDetected = true;
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isDetected = true;
    }
}
