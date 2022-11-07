using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerSpace;
using Photon.Pun;
public class TickDamage : MonoBehaviourPun
{
    [SerializeField]
    private float tickDamage = 1f;
    [SerializeField]
    private float damageTime = 3f;

    // 해당 bool 값은 반드시 파트너들끼리 통일이 되어야 합니다.
    [SerializeField]
    private bool isNestingCollision = false;
    [SerializeField]
    public List<GameObject> partnerObject;
    public List<GameObject> PartnerObject { get { return partnerObject; } }

    public Dictionary<string , bool> willDamageApply = new Dictionary<string, bool>();
    public Dictionary<string,bool> WillDamageApply { get { return willDamageApply; } }
    private Dictionary<string, Transform> damageTransform = new Dictionary<string, Transform>();

    [SerializeField] private bool haveOwner;
    public string OwnerID;
    public float ultimatePoint;


    [PunRPC]
    public void DamageApply(string userID,float damage)
    {
        if (userID == OwnerID)
            return;

        FloatingText.Instance.CreateFloatingTextForDamage(damageTransform[userID].position, damage);

        if (haveOwner)
        {
            PlayerInfoManager.Instance.CurHpDecrease(OwnerID, userID, damage);
            PlayerInfoManager.Instance.CurSkillPtIncrease(OwnerID, ultimatePoint);
        }
        else
            PlayerInfoManager.Instance.CurHpDecrease(userID, damage);
    }

    IEnumerator InDamageZone(string userID, float time, float damage)
    {
        Debug.Log(userID + "는 데미지를 곧 받을거야");
        while (true)
        {
            // 이 부분 수정하기 , 나가면 바로 꺼지지 않고 , time 만큼 기다렸다가 데미지를 입음
            Debug.Log("난돌고있어!!");
            if (false == willDamageApply[userID])
            {
                Debug.Log(userID + "는 데미지존을 나갔어 코루틴을 멈출거야");
                willDamageApply.Remove(userID);
                damageTransform.Remove(userID);
                yield break;
            }
            yield return new WaitForSeconds(time);

            if (false == willDamageApply[userID])
                continue;
            
            if (!PhotonNetwork.OfflineMode)
                photonView.RPC(nameof(DamageApply), RpcTarget.All, userID, damage);
            else
                DamageApply(userID, damage);

            Debug.Log(userID + "는 데미지를 받았어");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (PhotonNetwork.IsMasterClient && !PhotonNetwork.OfflineMode)
        {
            if (other.gameObject.tag == "Player")
            {
                if(true == isNestingCollision)
                {
                    willDamageApply[other.name] = false;
                }
                else if(false == isNestingCollision && false == IsInOtherObject(other.name))
                {
                    willDamageApply[other.name] = false;
                }
            }
        }
        else
        {
            willDamageApply[other.name] = false;
        }
    }

    private bool IsInOtherObject(string name)
    {
        foreach(GameObject obj in PartnerObject)
        {
            Dictionary<string, bool> test = obj.GetComponent<TickDamage>().willDamageApply;
            if (true == obj.GetComponent<TickDamage>().willDamageApply.ContainsKey(name))
                return true;
        }
        return false;
    }


    private void OnTriggerEnter(Collider other)
    {
        // 데미지 타임을 딱 두고 , , , 딱대라 시간 지내면 때린다
        // 또 코루틴?
        if(PhotonNetwork.IsMasterClient && !PhotonNetwork.OfflineMode)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if(true == isNestingCollision)
                {
                    Debug.Log("데미지 중첩임 코루틴 시작할꺼야");
                    willDamageApply.Add(other.name,true);
                    damageTransform.Add(other.name, other.transform);
                    StartCoroutine(InDamageZone(other.gameObject.name, damageTime, tickDamage));
                }
                else if (false == isNestingCollision && false == IsInOtherObject(other.gameObject.name) && false == willDamageApply[other.gameObject.name])
                {
                    Debug.Log("데미지 중첩아님 코루틴 시작할꺼야");
                    willDamageApply.Add(other.name, true);
                    damageTransform.Add(other.name, other.transform);

                    StartCoroutine(InDamageZone(other.gameObject.name, damageTime, tickDamage));
                }
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (true == isNestingCollision)
                {
                    willDamageApply.Add(other.name, true);
                    damageTransform.Add(other.name, other.transform);

                    StartCoroutine(InDamageZone(other.gameObject.name, damageTime, tickDamage));
                }
                else if (false == isNestingCollision && false == IsInOtherObject(other.gameObject.name))
                {
                    willDamageApply.Add(other.name, true);
                    damageTransform.Add(other.name, other.transform);

                    StartCoroutine(InDamageZone(other.gameObject.name, damageTime, tickDamage));
                }
            }
        }
    }
    



}
