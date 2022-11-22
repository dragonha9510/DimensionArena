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
    [SerializeField]
    private bool startDamage;

    // 해당 bool 값은 반드시 파트너들끼리 통일이 되어야 합니다.
    [SerializeField]
    private bool isNestingCollision = false;
    [SerializeField]
    public List<GameObject> partnerObject;
    public List<GameObject> PartnerObject { get { return partnerObject; } }

    public Dictionary<string , bool> willDamageApply = new Dictionary<string, bool>();
    public Dictionary<string,bool> WillDamageApply { get { return willDamageApply; } }

    public Dictionary<GameObject, bool> willDamageApplyBox = new Dictionary<GameObject, bool>();
    public Dictionary<GameObject, bool> WillDamageApplyBox { get { return willDamageApplyBox; } }


    public string OwnerID;
    public float ultimatePoint;

    [SerializeField] private bool isPercent;

    [PunRPC]
    public void DamageApply(string userID,float damage)
    {
        if (userID.Equals(OwnerID) || !PlayerInfoManager.Instance.DicPlayerInfo[userID].IsAlive)
            return;

        int decreaseHP;
        if (isPercent)
            decreaseHP = PlayerInfoManager.Instance.CurHPDecreaseRatio(OwnerID, userID, damage);
        else
            decreaseHP = PlayerInfoManager.Instance.CurHpDecrease(OwnerID, userID, damage);

        FloatingText.Instance.CreateFloatingTextForDamage(PlayerInfoManager.Instance.getPlayerTransform(userID).position, decreaseHP);
        PlayerInfoManager.Instance.CurSkillPtIncrease(OwnerID, ultimatePoint);
      
    }

    public void DamageApplyItemBox(GameObject itemBox, float damage)
    {
        if (isPercent)
            itemBox.GetComponent<ItemBox>().GetTickDamage(true,damage);
        else
            itemBox.GetComponent<ItemBox>().GetTickDamage(false, damage);
    }

    IEnumerator InDamageZone(GameObject obj, float time, float damage)
    {
        if (startDamage)
        {
            DamageApplyItemBox(obj,damage);
        }

        while (true)
        {
            // 이 부분 수정하기 , 나가면 바로 꺼지지 않고 , time 만큼 기다렸다가 데미지를 입음
            if (false == willDamageApplyBox[obj])
            {
                willDamageApplyBox.Remove(obj);
                yield break;
            }
            yield return new WaitForSeconds(time);

            if (false == willDamageApplyBox[obj])
                continue;

            DamageApplyItemBox(obj, damage);

        }
    }
    IEnumerator InDamageZone(string userID, float time, float damage)
    {
        if (startDamage)
        {
            if (!PhotonNetwork.OfflineMode)
            {
                photonView.RPC(nameof(DamageApply), RpcTarget.All, userID, damage);
            }
            else
            {
                DamageApply(userID, damage);
            }
        }

        while (true)
        {
            // 이 부분 수정하기 , 나가면 바로 꺼지지 않고 , time 만큼 기다렸다가 데미지를 입음
            if (false == willDamageApply[userID])
            {
                willDamageApply.Remove(userID);
                yield break;
            }
            yield return new WaitForSeconds(time);

            if (false == willDamageApply[userID])
                continue;
            
            if (!PhotonNetwork.OfflineMode)
                photonView.RPC(nameof(DamageApply), RpcTarget.All, userID, damage);
            else
                DamageApply(userID, damage);

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
            else if (other.gameObject.tag == "Item_Box")
            {
                if (true == isNestingCollision)
                {
                    willDamageApplyBox[other.gameObject] = false;
                }
                else if (false == isNestingCollision && false == IsInOtherObject(other.gameObject))
                {
                    willDamageApplyBox[other.gameObject] = false;
                }
            }
        }
        else
        {
            if(other.gameObject.tag == "Player")
                willDamageApply[other.name] = false;
            else if (other.gameObject.tag == "Item_Box")
                willDamageApplyBox[other.gameObject] = false;
        }
    }

    private bool IsInOtherObject(string name)
    {
        foreach(GameObject obj in PartnerObject)
        {
            if (true == obj.GetComponent<TickDamage>().WillDamageApply.ContainsKey(name))
                return true;
        }
        return false;
    }
    private bool IsInOtherObject(GameObject obj)
    {
        foreach (GameObject partner in PartnerObject)
        {
            if (true == partner.GetComponent<TickDamage>().WillDamageApplyBox.ContainsKey(obj))
                return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 데미지 타임을 딱 두고 , , , 딱대라 시간 지내면 때린다
        // 또 코루틴?
        if (other.gameObject.name.Equals(OwnerID))
            return;

        if(PhotonNetwork.IsMasterClient && !PhotonNetwork.OfflineMode)
        {
            if (other.CompareTag("Player"))
            {
                if(true == isNestingCollision)
                {
                    Debug.Log("데미지 중첩임 코루틴 시작할꺼야");

                    if (willDamageApply.ContainsKey(other.name))
                    {
                        willDamageApply[other.name] = true;
                        return;
                    }
                    willDamageApply.Add(other.name,true);
                    StartCoroutine(InDamageZone(other.gameObject.name, damageTime, tickDamage));
                }
                else if (false == isNestingCollision && false == IsInOtherObject(other.gameObject.name) && false == willDamageApply.ContainsKey(other.gameObject.name))
                {
                    Debug.Log("데미지 중첩아님 코루틴 시작할꺼야");
                    willDamageApply.Add(other.name, true);

                    StartCoroutine(InDamageZone(other.gameObject.name, damageTime, tickDamage));
                }
            }
            if(other.CompareTag("Item_Box"))
            {
                if (true == isNestingCollision)
                {
                    if (willDamageApplyBox.ContainsKey(other.gameObject))
                    {
                        willDamageApplyBox[other.gameObject] = true;
                        return;
                    }
                    willDamageApplyBox.Add(other.gameObject, true);
                    StartCoroutine(InDamageZone(other.gameObject, damageTime, tickDamage));
                }
                else if (false == isNestingCollision && false == IsInOtherObject(other.gameObject) && false == willDamageApplyBox.ContainsKey(other.gameObject))
                {
                    Debug.Log("데미지 중첩아님 코루틴 시작할꺼야");
                    willDamageApplyBox.Add(other.gameObject, true);
                    StartCoroutine(InDamageZone(other.gameObject, damageTime, tickDamage));
                }
            }
        }
        else
        {
            if (true == PhotonNetwork.InRoom)
                return;
            if (other.CompareTag("Player"))
            {
                if (true == isNestingCollision)
                {
                    willDamageApply.Add(other.name, true);
                    StartCoroutine(InDamageZone(other.gameObject.name, damageTime, tickDamage));
                }
                else if (false == isNestingCollision && false == IsInOtherObject(other.gameObject.name))
                {
                    willDamageApply.Add(other.name, true);
                    StartCoroutine(InDamageZone(other.gameObject.name, damageTime, tickDamage));
                }
            }
        }
    }
    



}
