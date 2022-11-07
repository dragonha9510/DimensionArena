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

    // �ش� bool ���� �ݵ�� ��Ʈ�ʵ鳢�� ������ �Ǿ�� �մϴ�.
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
        Debug.Log(userID + "�� �������� �� �����ž�");
        while (true)
        {
            // �� �κ� �����ϱ� , ������ �ٷ� ������ �ʰ� , time ��ŭ ��ٷȴٰ� �������� ����
            Debug.Log("�������־�!!");
            if (false == willDamageApply[userID])
            {
                Debug.Log(userID + "�� ���������� ������ �ڷ�ƾ�� ����ž�");
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

            Debug.Log(userID + "�� �������� �޾Ҿ�");
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
        if(PhotonNetwork.IsMasterClient && !PhotonNetwork.OfflineMode)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if(true == isNestingCollision)
                {
                    Debug.Log("������ ��ø�� �ڷ�ƾ �����Ҳ���");
                    willDamageApply.Add(other.name,true);
                    damageTransform.Add(other.name, other.transform);
                    StartCoroutine(InDamageZone(other.gameObject.name, damageTime, tickDamage));
                }
                else if (false == isNestingCollision && false == IsInOtherObject(other.gameObject.name) && false == willDamageApply[other.gameObject.name])
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
