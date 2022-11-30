using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using ManagerSpace;
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

    private float maxHealth;

    public float Health => health;
    [SerializeField]
    private TextMeshProUGUI itemHealth;
    [SerializeField]
    private Slider itemSlider;


    private ITEM makeItemType = ITEM.ITEM_END;
    private string itemPrefabName = "";


    private void Start()
    {
        maxHealth = health;
        if (itemHealth != null)
            itemHealth.text = health.ToString();
        if(itemSlider != null)
        {
            itemSlider.maxValue = health;
            itemSlider.value = health;
        }
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
  
    public void GetTickDamage(bool isPercent,float dmg , string ownerID = "")
    {
        int damage;
        if (isPercent)
            damage = (int)(maxHealth * dmg);
        else
            damage = (int)dmg;
        photonView.RPC(nameof(HpDecrease), RpcTarget.All, damage, ownerID);
    }

    public void GetDamage(float dmg, string ownerID = "")
    {
        photonView.RPC(nameof(HpDecrease), RpcTarget.All, (int)dmg, ownerID);

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

    private void MakeRandItem(string owner)
    {
        if (PhotonNetwork.InRoom)
        {
            GameObject item = PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_ITEMPREFABFOLDER + itemPrefabName, this.transform.position, Quaternion.identity);
            item.GetComponent<Item>().enabled = true;
            item.GetComponent<Item>().ownerName = owner;
        }
        else if (!PhotonNetwork.InRoom)
            return;
    }
    [PunRPC]
    private void HpDecrease(int damage, string owner)
    {
        health -= damage;
        if(itemSlider != null)
            itemSlider.value = health;
        if(itemHealth != null)
            itemHealth.text = health.ToString();
        if(null != GetComponentInChildren<Shaking>())
        {
            if (false == GetComponentInChildren<Shaking>().IsShaking)
                GetComponentInChildren<Shaking>().StartShaking();
        }
        if (health <= 0)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            SoundManager.Instance.PlaySFXOneShotInRange(60f, this.transform, "ItemBoxBreak");
            MakeRandItem(owner);
            photonView.RPC(nameof(CreateDestroyEfffect), RpcTarget.All, this.transform.position, this.transform.rotation, "ItemBox");
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    [PunRPC]
    private void CreateDestroyEfffect(Vector3 pos,Quaternion rot,string eventType)
    {
        EffectManager.Instance.CreateParticleEffectOnGameobject(pos, rot, "ItemBox");
    }
    [PunRPC]
    public void HpDecrease_KnockBack(float damage, string owner)
    {
        if (!PhotonNetwork.OfflineMode)
            photonView.RPC(nameof(HpDecrease), RpcTarget.All, (int)damage, owner);
        else
            HpDecrease((int)damage, owner);
    }
}
