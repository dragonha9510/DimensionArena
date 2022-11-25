using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;
using ManagerSpace;
using PlayerSpace;

public class Prototype_TargetCamera : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Transform tempTarget;
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
        Vector3 location = target.position + interval;
        location.y = transform.position.y;
        transform.DOMove(location, arrivalTime);

        //���߿� Ȯ�� waitforseconds ����
        Invoke(nameof(OnStart), arrivalTime);
    }

    public void ChanageTarget(string ownerName, string targetName)
    {
        PlayerInfoManager infoMgr = PlayerInfoManager.Instance;

        //�ٸ� �÷��̾� ��� ó���� �����ʴ´�.
        if(infoMgr.DicPlayer.ContainsKey(ownerName))
            if (infoMgr.DicPlayer[ownerName].transform != target)
                return;

        if (infoMgr.SurvivalCount >= 1)
        {
            //���� ����� �ִٸ�
            if (infoMgr.DicPlayer.ContainsKey(targetName))
                tempTarget = infoMgr.getPlayerTransform(targetName);
            else
            {
                foreach(var player in infoMgr.DicPlayer.Values)
                {
                    if (player.activeInHierarchy)
                        tempTarget = infoMgr.getPlayerTransform(player.name);
                }
            }
        }
        else
        {
            InGameUIManager.Instance.LoadResultScene();
        }

    }

    private void OnStart()
    {
        isStartEnd = true;
    }

}
