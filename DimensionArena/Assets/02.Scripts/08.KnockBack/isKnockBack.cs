using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ManagerSpace;
public class isKnockBack : MonoBehaviourPun
{
    [HideInInspector] public KnockBackInfo info;
    private float curSpeed;
    private float realSpeed;
    private float curDistance;
    private void Start()
    {
        
    }

    public void SetValue()
    {
        curDistance = 0;
        curSpeed = realSpeed = info.speed;
    }

    /*private void Update()
    {
        if (info.isOn == false)
            return;
       if(!PhotonNetwork.InRoom)
        {
            float speed = curSpeed * Time.deltaTime;
            curDistance += speed;

            transform.position += info.direction.normalized * speed;

            if (info.distance <= curDistance)
            { 
                info.isOn = false;
                this.enabled = false;
            }
            else
                curSpeed -= realSpeed * Time.deltaTime * curDistance;
        }
       else if(PhotonNetwork.InRoom)
        {
            if(info.distance<=curDistance)
            {
                info.isOn = false;
                this.enabled = false;
            }
            else
                photonView.RPC(nameof(MoveKnockBack), RpcTarget.All);
        }
    }

    [PunRPC]
    private void MoveKnockBack()
    {
        float speed = curSpeed * Time.deltaTime;
        curDistance += speed;

        transform.position += info.direction.normalized * speed;

        if (info.distance <= curDistance)
        {
            info.isOn = false;
            this.enabled = false;
        }
        else
            curSpeed -= realSpeed * Time.deltaTime * curDistance;
    }*/

    public void AllClientKncokBackCall(string ID, Vector3 pos, Vector3 dir, float speed, float distance)
    {
        photonView.RPC(nameof(AllClientKncokBack), RpcTarget.All,ID, pos, dir, speed,distance);
    }

    [PunRPC]
    private void AllClientKncokBack(string ID, Vector3 pos, Vector3 dir, float speed, float distance)
    {
        PlayerInfoManager.Instance.DicPlayer[ID].GetComponent<isKnockBack>().CallMoveKnockBack(pos, dir, speed, distance);
    }

    public void CallMoveKnockBack(Vector3 pos,Vector3 direction, float speed, float knockbackDistance)
    {
        StartCoroutine(MoveKnockBack(pos,direction, speed, knockbackDistance));
    }

    private IEnumerator MoveKnockBack(Vector3 pos,Vector3 direction , float speed , float knockbackDistance)
    {
        while(true)
        {
            if (Vector3.Distance(pos, this.transform.position) > knockbackDistance)
                yield break;
            else
            {
                this.transform.position = this.transform.position + direction * speed * Time.fixedDeltaTime;
                yield return null;
            }
        }
    }

}

