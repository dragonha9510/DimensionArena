using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;

public class DetectArea : MonoBehaviour
{
    [SerializeField] private SphereCollider sphareCollide;
    [SerializeField] private Transform skillUiRadius;
    List<GameObject> listTarget = new List<GameObject>();
    [SerializeField] Transform target;
    public Transform Target => target;
    bool isTargetDetect;
    public bool IsTargetDetect => isTargetDetect;
    private Vector3[] targetPos = new Vector3[8];
    public Vector3[] TargetPos => targetPos;
   
    private int collisionlayer;
    public  int CollisionLayer => collisionlayer;

    private bool isMine;
    
    private void Start()
    {
        isMine = GetComponentInParent<Player>().photonView.IsMine;
        collisionlayer = (1 << LayerMask.NameToLayer("Obstacle") 
                        | 1 << LayerMask.NameToLayer("Player") 
                        | 1 << LayerMask.NameToLayer("Item_Box"));
    }

    public void SetRadius(float range)
    {
        skillUiRadius.localScale = Vector3.one * range;
        sphareCollide.radius = range * 0.5f;
    }

    private void Update()
    {
        //내꺼가 아니라면 업데이트 체크 X
        if (!isMine)
            return;

        if (listTarget.Count < 1)
        {
            isTargetDetect = false;
            target = null;
            return;
        }

        if (!listTarget[0].gameObject.activeInHierarchy)
            listTarget.RemoveAt(0);

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
        targetPos[0] = target.position + target.forward;
        targetPos[1] = target.position + (target.forward + target.right).normalized;
        targetPos[2] = target.position + (target.forward - target.right).normalized; ;
        targetPos[3] = target.position + target.right;
        targetPos[4] = target.position - target.right;
        targetPos[5] = target.position + (-target.forward + target.right).normalized;
        targetPos[6] = target.position + (-target.forward - target.right).normalized;
        targetPos[7] = target.position - target.forward;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.GetComponentInChildren<isHideOnBush>().isHide)
            return;

        listTarget.Add(other.gameObject);
    }


    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.GetComponentInChildren<isHideOnBush>().isHide)
        {
            listTarget.Remove(other.gameObject);
        }
        else
        {
            if(!listTarget.Contains(other.gameObject))
                listTarget.Add(other.gameObject);
        }
            return;

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        listTarget.Remove(other.gameObject);
    }

}
