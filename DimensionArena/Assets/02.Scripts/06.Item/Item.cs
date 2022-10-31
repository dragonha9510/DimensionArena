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

    Vector3 randBoing;
    private int ColliderCount = 0;
    private int StopCount = 3;

    private Transform trans;
    public Transform Trans { get { return this.transform; } }
    // Start is called before the first frame update
    void Start()
    {
            trans = this.transform;
            randBoing = new Vector3(Random.Range(-0.2f, 0.2f), 1, Random.Range(-0.2f, 0.2f));

            rigidBody = GetComponent<Rigidbody>();
            rigidBody.AddForce(randBoing.normalized * boingPower, ForceMode.Impulse);

    }


    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        this.transform.Rotate(Vector3.up * rotation * Time.deltaTime, Space.World);
        if (0 >= info.liveTime)
            PhotonNetwork.Destroy(this.gameObject);
        else
            info.liveTime -= Time.fixedDeltaTime;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if (collision.gameObject.tag == "ParentGround")
        {
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
        else if(collision.gameObject.tag == "Player")
        {
            EffectManager.Instance.CreateParticleEffectOnGameobject(collision.gameObject.transform, "ItemDrop");
            InteractItem(collision.gameObject.name);
            // 아이템 이벤트 처리
           
            Destroy(this.gameObject);
        }

    }
    public void SettingItem(ItemInfo itemInfo)
    {
        info = itemInfo;
    }


    protected abstract void InteractItem(string targetID);


}
