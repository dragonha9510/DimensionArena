using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;

public class DetectArea : MonoBehaviour
{
    [SerializeField] private SphereCollider sphareCollide;
    List<GameObject> listTarget = new List<GameObject>();
    [SerializeField] Transform target;
    public Transform Target => target;
    bool isTargetDetect;
    public bool IsTargetDetect => isTargetDetect;

    private void Start()
    {
        Player_Skill skill = GetComponentInParent<Player_Skill>();
        sphareCollide.radius = skill.MaxRange;
    }

    private void Update()
    {
        if (listTarget.Count < 1)
        {
            isTargetDetect = false;
            target = null;
            return;
        }

        listTarget.Sort(delegate (GameObject A, GameObject B)
        {
            float Adistance = Vector3.Distance(transform.position, A.transform.position);
            float Bdistance = Vector3.Distance(transform.position, B.transform.position);

            if (Adistance < Bdistance) return -1;
            else if (Adistance > Bdistance) return 1;

            return 0;
        });

        isTargetDetect = true;
        target = listTarget[0].transform;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        listTarget.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        listTarget.Remove(other.gameObject);
    }

}
