using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;
using ManagerSpace;
using Photon.Pun;

public enum ITEM
{
    ITEM_MEDICKIT,
    ITEM_POWERKIT,
    ITEM_SHIELDKIT,
    ITEM_SPEEDKIT,
    ITEM_ENERGYKIT,
    ITEM_DEMENSIONKIT,
    ITEM_END
}

public abstract class Item : MonoBehaviourPun
{

    // For Parshing
    [SerializeField]
    protected ItemInfo info;

    private Rigidbody rigidBody;
    [SerializeField]
    private float boingPower = 1.0f;
    [SerializeField]
    private float decreasePower = 0.4f;
    [SerializeField]
    private float rotation = 100.0f;
    [SerializeField]
    private float decreaseRotation = 0.9f;
    [SerializeField]
    private GameObject particle;

    [SerializeField]
    private float liveTime = 20.0f;

    private float curTime;
    public float CurTime => curTime;
    public string ownerName;

    Vector3 randBoing;
    private int ColliderCount = 0;
    private int StopCount = 3;


    private Vector3 eatTrans;
    private Quaternion eatRot;

    private Transform trans;
    public Transform Trans { get { return this.transform; } }
    // Start is called before the first frame update
    void Start()
    {
        trans = this.transform;
        randBoing = new Vector3(Random.Range(-0.2f, 0.2f), 1, Random.Range(-0.2f, 0.2f));

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(randBoing.normalized * boingPower, ForceMode.Impulse);
        Detector dectector = GetComponentInChildren<Detector>();
        dectector.item = this;
    }


    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        this.transform.Rotate(Vector3.up * rotation * Time.deltaTime, Space.World);

        if (0 >= liveTime)
            PhotonNetwork.Destroy(this.gameObject);
        else
        {
            curTime += Time.fixedDeltaTime;
            liveTime -= Time.fixedDeltaTime;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!PhotonNetwork.IsMasterClient || null == rigidBody)
            return;

        if (collision.gameObject.tag == "ParentGround")
        {
            Debug.Log("아이템 콜리전 엔터");
            if (ColliderCount <= StopCount)
            {
                boingPower *= decreasePower;
                rigidBody.AddForce(randBoing.normalized * boingPower, ForceMode.Impulse);
                ++ColliderCount;
                rotation *= decreaseRotation;
            }
            if (ColliderCount == StopCount)
            {
                rigidBody.freezeRotation = true;
                particle.SetActive(true);
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            //5초동안 아이템 우선권 부여
            if (curTime <= 5.0f)
            {
                if (!ownerName.Equals(collision.gameObject.name))
                    return;
            }
            // 이펙트 생성을 위한 설정
            eatTrans = this.trans.position;
            eatRot = this.trans.rotation;
            // 
            if (PhotonNetwork.IsMasterClient)
                InteractItem(collision.gameObject.name);
        }
    }

    [PunRPC]
    protected void CreateDropEffect()
    {
        EffectManager.Instance.CreateParticleEffectOnGameobject(eatTrans, eatRot, "ItemDrop");
    }


    public void SettingItem(ItemInfo itemInfo)
    {
        info = itemInfo;
    }


    protected abstract void InteractItem(string targetID);


}
