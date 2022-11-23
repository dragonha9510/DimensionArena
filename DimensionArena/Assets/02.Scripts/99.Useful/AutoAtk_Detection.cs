using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;
using Photon.Pun;

[RequireComponent(typeof(SphereCollider))]
public class AutoAtk_Detection : MonoBehaviour
{
    [SerializeField] private bool isSkill = false;
    private Player_Atk info;
    private Player_Skill info_skill;

    public List<Transform> detectedTransform;
    public List<isHideOnBush> detectedIsHide;

    public Vector3 targetPos;
    private float range;
    private SphereCollider detect_collider;

    private void Start()
    {
        detect_collider = GetComponent<SphereCollider>();

        if (isSkill)
            info_skill = GetComponentInParent<Player_Skill>();
        else
            info = GetComponentInParent<Player_Atk>();

        range = (isSkill ? info_skill.MaxRange : info.AtkInfo.Range);
        detect_collider.radius = range;

        targetPos = transform.position + transform.forward * range;
    }

    private void Update()
    {
        if (!PhotonNetwork.OfflineMode && !GameManager.Instance.IsSpawnEnd)
            return;

        targetPos = transform.position + transform.forward * range;

        if (detectedTransform.Count == 0)
            return;
        else
            detectedTransform.RemoveAll(s => s == null);
        
        detectedTransform.Sort(delegate (Transform A, Transform B)
        {
            float Adistance = Vector3.Distance(transform.position, A.position);
            float Bdistance = Vector3.Distance(transform.position, B.position);

            if (Adistance < Bdistance) return -1;
            else if (Adistance > Bdistance) return 1;

            return 0;
        });

        if (NullCheck.IsNullOrEmpty(detectedTransform) || detectedTransform[0] == null)
            return;

        isHideOnBush tryCheck = null;
        bool AllTargetInBush = true;

        foreach (var isHide in detectedTransform)
        {
            tryCheck = isHide.GetComponentInChildren<isHideOnBush>();

            if(tryCheck == null || !tryCheck.isHide)
            {
                AllTargetInBush = false;
                 
                targetPos = detectedTransform[0].position;
                break;
            }
        }

        if(AllTargetInBush)
            targetPos = transform.position + transform.forward * range;

        targetPos.y = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" )
            return;
        
        detectedTransform.Add(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        detectedTransform.Remove(other.transform);
    }
}
