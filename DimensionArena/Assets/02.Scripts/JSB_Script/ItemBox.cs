using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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

    private ITEM makeItemType = ITEM.ITEM_END;
    private string itemPrefabName = "";

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

    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_ITEMPREFABFOLDER + itemPrefabName, this.transform.position, Quaternion.identity);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
