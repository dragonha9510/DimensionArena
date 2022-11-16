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
        interval = transform.position;

        if (!PhotonNetwork.InRoom)
            FollowTargetAtGameStart();
    }
    private void LateUpdate()
    {      
        if (!isStartEnd)
            return;

        Debug.Log("들어옴");
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
        StartCoroutine(nameof(FollowTargetCoroutine));
    }


    private IEnumerator FollowTargetCoroutine()
    {
        while (true)
        {
            if (target != null) break;
            else yield return null;
        }
        Debug.Log("안녕");
        Vector3 location = target.position + interval;
        location.y = transform.position.y;
        transform.DOMove(location, arrivalTime);

        //나중에 확인 waitforseconds 실패
        Invoke(nameof(OnStart), arrivalTime);
    }

    private void OnStart()
    {
        isStartEnd = true;
        Debug.Log("시작");
    }
}
