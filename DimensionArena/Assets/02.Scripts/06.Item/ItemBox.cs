using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class ItemBox : MonoBehaviourPun
{
    [System.Serializable]
    struct dropTable
    {
        public ITEM itemType;
        public float minPercentRange;
        public float maxPrecentRange;
        public float percent;
        public bool dropPossible;
    }
    [SerializeField]
    private List<dropTable> dropTables = new List<dropTable>();
    [SerializeField]
    private float health;
    [SerializeField]
    private TextMeshProUGUI itemHealth;
    [SerializeField]
    private Slider itemSlider;

    


    private ITEM makeItemType = ITEM.ITEM_END;
    private string itemPrefabName = "";


    private void Start()
    {
        itemHealth.text = health.ToString();
        itemSlider.maxValue = health;
        itemSlider.value = health;
    }

    private void OnEnable()
    {
        float rand = Random.Range(0.0f, 1.0f);
        for(int i = 0;i < (int)ITEM.ITEM_END;++i)
        {
            if(dropTables[i].minPercentRange <= rand && rand < dropTables[i].maxPrecentRange)
            {
                makeItemType = dropTables[i].itemType;
                switch (makeItemType)
                {
                    case ITEM.ITEM_MEDICKIT:
                        itemPrefabName = "MedicKit";
                        break;
                    case ITEM.ITEM_POWERKIT:
                        itemPrefabName = "PowerKit";
                        break;
                    case ITEM.ITEM_SHIELDKIT:
                        itemPrefabName = "ShieldKit";
                        break;
                    case ITEM.ITEM_SPEEDKIT:
                        itemPrefabName = "SpeedKit";
                        break;
                    case ITEM.ITEM_ENERGYKIT:
                        itemPrefabName = "EnergyKit";
                        break;
                    case ITEM.ITEM_DEMENSIONKIT:
                        itemPrefabName = "DemensionKit";
                        break;
                }
                return;
            }
        }
    }

    public void ResetPrefab()
    {
        dropTables.Clear();
    }

    public void SetDropTable(ITEM itemType, bool drop_Possible, float percentage)
    {
        dropTable droptable;
        droptable.itemType = itemType;
        droptable.dropPossible= drop_Possible;
        droptable.percent = percentage;

        if(dropTables.Count == 0)
        {
            droptable.minPercentRange = 0.0f;
            droptable.maxPrecentRange = percentage;
        }
        else
        {
            droptable.minPercentRange = dropTables[dropTables.Count - 1].maxPrecentRange;
            droptable.maxPrecentRange = droptable.minPercentRange + percentage;
        }
        dropTables.Add(droptable);
    }

    // 주어진 float 0 ~ 1 까지의 확률
    // 0 ~ 1 까지 float 으로 랜덤을 돌린다 . 

    private void MakeRandItem()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        GameObject Test = PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_ITEMPREFABFOLDER + itemPrefabName, this.transform.position, Quaternion.identity);
        PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    private void HealthDecrease(int damage)
    {
        health -= damage;
        itemSlider.value = health;
        if (false == GetComponentInChildren<Shaking>().IsShaking)
            GetComponentInChildren<Shaking>().StartShaking();
        if (health <= 0)
        {
            if(photonView.IsMine)
            {
                MakeRandItem();
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            if (other.gameObject.tag == "AttackCollider")
            {
                photonView.RPC(nameof(HealthDecrease), RpcTarget.All, other.gameObject.GetComponent<AttackObject>().Damage);
            }
        }
        else
        {
            if (other.gameObject.tag == "AttackCollider")
            {
                health -= other.gameObject.GetComponent<Projectile>().Damage;
                itemSlider.value = health;
                if (health <= 0)
                {
                    MakeRandItem();
                    Destroy(this.gameObject);
                }
            }
        }
        
    }

  

}
