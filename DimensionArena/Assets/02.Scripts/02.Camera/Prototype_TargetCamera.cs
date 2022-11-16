using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class Prototype_TargetCamera : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] private float arrivalTime;
    private Vector3 interval;
    private bool isStartEnd;

    private void Awake()
    {
        if (PhotonNetwork.OfflineMode)
            isStartEnd = true;

        interval = transform.position;

        if (target == null)
        {
            GameObject temp = Instantiate(new GameObject(), transform);
            temp.transform.position = Vector3.zero;
            target = temp.transform;
            temp.name = "Dummy";
        }

        StartCoroutine(FollowTargetCoroutine());
    }

    private void FixedUpdate()
    {
        if (!isStartEnd)
            return;

        //transform.LookAt(target);
        Vector3 location = target.position + interval;
        location.y = transform.position.y;
        Vector3 direction = location - transform.position;

        float speed = direction.magnitude * 2;
        direction.Normalize();

        transform.position = transform.position + (direction * speed * Time.deltaTime);
    }



    public void FollowTargetAtGameStart()
    {
        StartCoroutine(FollowTargetCoroutine());
    }

    private IEnumerator FollowTargetCoroutine()
    {
        Vector3 location = target.position + interval;
        location.y = transform.position.y;
        transform.DOMove(location, arrivalTime);
        yield return new WaitForSeconds(arrivalTime);
        isStartEnd = true;
    }
}
