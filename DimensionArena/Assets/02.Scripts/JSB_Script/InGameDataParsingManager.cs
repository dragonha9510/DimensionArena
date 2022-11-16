using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class InGameDataParsingManager : MonoBehaviour
{

    [SerializeField]
    private string[] parsingWall = new string[3];


    private void Awake()
    {

        parsingWall[0] = "Wall1";
        parsingWall[1] = "Wall2";
        parsingWall[2] = "Wall3";

        
        ParsingToCsvFile_ItemBox("Log/Item_DB_Test");
        ParsingToCsbFile_Items("Log/Items");
    }

    

    private void ParsingToCsbFile_Items(string path)
    {
        Debug.Log("아이템 정보 세팅중");

        string itemResourcePath = "Assets/Resources/Prefabs/ProtoTypeItems/";
        List<GameObject> itemObjs = new List<GameObject>();
        List<Dictionary<string, object>> data_Map = CSVReader.Read(path);


        for(int i = 0; i < data_Map.Count; ++i)
        {
            if (data_Map[i]["item_num"].ToString()[0] == '#')
                continue;
            string itemPrefapPath = itemResourcePath + data_Map[i]["item_id"].ToString() + ".prefab";
            Object itemObj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(itemPrefapPath);
            GameObject item = Instantiate(itemObj) as GameObject;
            itemObjs.Add(item);

            SettingItemData(item
                            , data_Map[i]["item_num"].ToString(), data_Map[i]["item_id"].ToString()
                            , data_Map[i]["achieve_range"].ToString(), data_Map[i]["Max_duration"].ToString()
                            , data_Map[i]["attack_nesting_or_not"].ToString()
                            , data_Map[i]["attack_increment"].ToString(),data_Map[i]["speed_amount"].ToString(), data_Map[i]["recovery_amount"].ToString(), data_Map[i]["shield_amount"].ToString(), data_Map[i]["gauge_recovery_amount"].ToString());
            bool isSucces = false;
            UnityEditor.PrefabUtility.SaveAsPrefabAsset(item, itemPrefapPath, out isSucces);
            UnityEditor.AssetDatabase.Refresh();
        }


        foreach (GameObject obj in itemObjs)
        {
            Destroy(obj.gameObject);
        }

        itemObjs.Clear();
        Debug.Log("아이템 정보 세팅 끝");

    }

    private void SettingItemData(GameObject item
                            , string item_Num, string item_ID
                            , string acheive_Range, string duration_time
                            , string attack_Nesting
                            , string attack, string speed, string health, string shield, string skill)
    {
        ItemInfo tmpItemInfo = new ItemInfo();
        tmpItemInfo.itemNumber = item_Num;
        tmpItemInfo.item_ID = item_ID;
        float.TryParse(acheive_Range, out tmpItemInfo.achieveRange);
        float.TryParse(duration_time, out tmpItemInfo.statusDuration);

        tmpItemInfo.attackNesting = GetBool(attack_Nesting);

        float.TryParse(attack, out tmpItemInfo.attackIncrement);
        float.TryParse(speed, out tmpItemInfo.speedAmount);
        float.TryParse(health, out tmpItemInfo.healthAmount);
        float.TryParse(shield, out tmpItemInfo.shieldAmount);
        float.TryParse(skill, out tmpItemInfo.skillRecovery);

        item.GetComponent<Item>().SettingItem(tmpItemInfo);

    }

    private bool GetBool(string str)
    {
        switch(str)
        {
            case "TRUE":
                return true;
            case "FALSE":
                return false;
        }
        return false;
    }

    private void ParsingToCsvFile_WallBoxes(string path)
    {
        string wallResourcePath = "Assets/Resources/Prefabs/ProtoTypeItems/";
        List<GameObject> wallObjs = new List<GameObject>();
        List<Dictionary<string, object>> data_Map;
        Object tmpObject;
        GameObject tmpGameObject;
        foreach (string name in parsingWall)
        {
            tmpObject = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(wallResourcePath + name);
            tmpGameObject = Instantiate(tmpObject) as GameObject;
            data_Map = CSVReader.Read(path + name);

            for (int i = 0, counting = 0; i < data_Map.Count; ++i)
            {
                if (data_Map[i]["item_num"].ToString()[0] == '#')
                    continue;
                //Object Create
                SettingItemDropTable(tmpGameObject, counting, data_Map[i]["item_id"].ToString(),
                             data_Map[i]["drop_possible"].ToString(),
                             data_Map[i]["drop_percentage"].ToString());
                ++counting;
            }

        }

        foreach(GameObject obj in wallObjs)
        {
            Destroy(obj);
        }
    }


    private void ParsingToCsvFile_ItemBox(string path)
    {
        string itemBoxpath = "Assets/Resources/Prefabs/ProtoTypeItems/ItemBox.prefab";
        Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(itemBoxpath);
        GameObject itemBox = Instantiate(obj) as GameObject;

        itemBox.GetComponent<ItemBox>().ResetPrefab();

        List<Dictionary<string, object>> data_Map = CSVReader.Read(path);

        for (int i = 0 , counting = 0; i < data_Map.Count; ++i)
        {
            if (data_Map[i]["item_num"].ToString()[0] == '#')
                continue;
            //Object Create
            SettingItemDropTable(itemBox,counting, data_Map[i]["item_id"].ToString(),
                         data_Map[i]["drop_possible"].ToString(),
                         data_Map[i]["drop_percentage"].ToString());
            ++counting;

        }

        bool isSucces = false;
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(itemBox, itemBoxpath, out isSucces);
        UnityEditor.AssetDatabase.Refresh();
        Destroy(itemBox);
    }
    private void SettingItemDropTable(GameObject itemBox,int count ,string id,string boolean,string percent)
    {
        ITEM itemType = ITEM.ITEM_END;
        switch(id)
        {
            case "power_kit":
                itemType = ITEM.ITEM_POWERKIT;
                break;
            case "medic_kit":
                itemType = ITEM.ITEM_MEDICKIT;
                break;
            case "shield_kit":
                itemType = ITEM.ITEM_SHIELDKIT;
                break;
            case "speed_kit":
                itemType = ITEM.ITEM_SPEEDKIT;
                break;
            case "energy_kit":
                itemType = ITEM.ITEM_ENERGYKIT;
                break;
            case "dimension_kit":
                itemType = ITEM.ITEM_DEMENSIONKIT;
                break;
        }
        bool dropPossible = GetBool(boolean);
        float dropPercent = float.Parse(percent);
        itemBox.GetComponent<ItemBox>().SetDropTable(itemType,dropPossible, dropPercent);

        

    }

}
#endif