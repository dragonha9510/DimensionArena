using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class KnockBackObject : MonoBehaviourPun
{
    [SerializeField] private KnockBackInfo info;
    [SerializeField] private GameObject knockBack;
    [SerializeField] private float triggerRadius;
    private GameObject knockBack_create;
    private KnockBack knockBack_info;
    // Start is called before the first frame update

    void Start()
    {
        if (!PhotonNetwork.InRoom)
        {
            knockBack_create = Instantiate(knockBack, transform.position, knockBack.transform.rotation);
            knockBack_info = knockBack_create.GetComponent<KnockBack>();
            knockBack_info.info = info;
            knockBack_create.GetComponent<SphereCollider>().radius = triggerRadius;

            knockBack_create.SetActive(false);
        }
        else if(PhotonNetwork.InRoom)
        {
            knockBack_create = Instantiate(knockBack, transform.position, knockBack.transform.rotation);
            knockBack_info = knockBack_create.GetComponent<KnockBack>();
            knockBack_info.info = info;
            knockBack_create.GetComponent<SphereCollider>().radius = triggerRadius;
            knockBack_create.SetActive(false);
        }
    }

    public void KnockBackStart()
    {
        if(!PhotonNetwork.InRoom)
        {
            knockBack_create.transform.position = transform.position;
            knockBack_create.SetActive(true);
        }
        else if(PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            knockBack_create.transform.position = transform.position;
            knockBack_create.SetActive(true);
        }
    }
    public void KnockBackStartDamage(string ownerID, float damage, float ultimatePoint = 0)
    {
        if(!PhotonNetwork.InRoom)
        {
            knockBack_create.transform.position = transform.position;
            knockBack_create.SetActive(true);

            knockBack_info.info.ownerID = ownerID;
            knockBack_info.info.damage = damage;
            knockBack_info.info.ultimatePoint = ultimatePoint;
        }
        if(PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            knockBack_create.transform.position = transform.position;
            knockBack_create.SetActive(true);

            knockBack_info.info.ownerID = ownerID;
            knockBack_info.info.damage = damage;
            knockBack_info.info.ultimatePoint = ultimatePoint;
        }
    }


    private void OnDestroy()
    {
        //KnockBackStart();
    }

    private void OnDisable()
    {
        //KnockBackStart();
    }
}
