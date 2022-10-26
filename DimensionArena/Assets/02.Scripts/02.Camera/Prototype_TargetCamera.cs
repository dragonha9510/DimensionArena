using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Prototype_TargetCamera : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] private float arrivalTime;
    private Vector3 interval;
    private bool isStartEnd;

    private void Start()
    {
        if (PhotonNetwork.OfflineMode)
            isStartEnd = true;

            interval = transform.position;

        if (target == null)
        {
            GameObject temp = Instantiate(new GameObject(), transform);
            temp.transform.position = Vector3.zero;

            target = temp.transform;
        }
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
        float time = 0.0f;
        
        while (true)
        {
            time += Time.unscaledTime;
            Vector3 location = target.position + interval;
            location.y = transform.position.y;
            Vector3 direction = location - transform.position;
            if (arrivalTime <= time) break;
            float speed = direction.magnitude / (arrivalTime - time);
            Debug.Log(speed);
            direction.Normalize();
            transform.position = transform.position + (direction * speed * Time.deltaTime);
            yield return null;
        }
        isStartEnd = true;
    }
}
