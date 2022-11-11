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

    // �ش� bool ���� �ݵ�� ��Ʈ�ʵ鳢�� ������ �Ǿ�� �մϴ�.
    [SerializeField]
    private bool isNestingCollision = false;
    [SerializeField]
    public List<GameObject> partnerObject;
    public List<GameObject> PartnerObject { get { return partnerObject; } }

    public Dictionary<string , bool> willDamageApply = new Dictionary<string, bool>();
    public Dictionary<string,bool> WillDamageApply { get { return willDamageApply; } }
    private Dictionary<string, Transform> damageTransform = new Dictionary<string, Transform>();

    public string OwnerID;
    public float ultimatePoint;

    [SerializeField] private bool isPercent;

    [PunRPC]
    public void DamageApply(string userID,float damage)
    {
        if (userID.Equals(OwnerID) || !PlayerInfoManager.Instance.DicPlayer.GetValueOrDefault(userID).activeInHierarchy)
            return;

        int decreaseHP;
        if (isPercent)
            decreaseHP = PlayerInfoManager.Instance.CurHPDecreaseRatio(OwnerID, userID, damage);
        else
            decreaseHP = PlayerInfoManager.Instance.CurHpDecrease(OwnerID, userID, damage);

        //damageTransform.GetValueOrDefault(userID)
        FloatingText.Instance.CreateFloatingTextForDamage(damageTransform[userID].position, decreaseHP);
        PlayerInfoManager.Instance.CurSkillPtIncrease(OwnerID, ultimatePoint);
      
    }

    IEnumerator InDamageZone(string userID, float time, float damage)
    {

        if (startDamage)
        {
            if (!PhotonNetwork.OfflineMode)
                photonView.RPC(nameof(DamageApply), RpcTarget.All, userID, damage);
            else
                DamageApply(userID, damage);
        }

        while (true)
        {
            // �� �κ� �����ϱ� , ������ �ٷ� ������ �ʰ� , time ��ŭ ��ٷȴٰ� �������� ����
            if (false == willDamageApply[userID])
            {
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
        // ������ Ÿ���� �� �ΰ� , , , ����� �ð� ������ ������
        // �� �ڷ�ƾ?
        if (other.gameObject.name.Equals(OwnerID))
            return;

        if(PhotonNetwork.IsMasterClient && !PhotonNetwork.OfflineMode)
        {
            if (other.CompareTag("Player"))
            {
                if(true == isNestingCollision)
                {
                    Debug.Log("������ ��ø�� �ڷ�ƾ �����Ҳ���");
                    willDamageApply.Add(other.name,true);
                    damageTransform.Add(other.name, other.transform);
                    StartCoroutine(InDamageZone(other.gameObject.name, damageTime, tickDamage));
                }
                else if (false == isNestingCollision && false == IsInOtherObject(other.gameObject.name) && false == willDamageApply.ContainsKey(other.gameObject.name))
                {
                    Debug.Log("������ ��ø�ƴ� �ڷ�ƾ �����Ҳ���");
                    willDamageApply.Add(other.name, true);
                    damageTransform.Add(other.name, other.transform);

                    StartCoroutine(InDamageZone(other.gameObject.name, damageTime, tickDamage));
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
