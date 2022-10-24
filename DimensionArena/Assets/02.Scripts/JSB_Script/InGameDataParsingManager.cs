using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class InGameDataParsingManager : MonoBehaviour
{

    private void Awake()
    {
        ParsingToCsvFile_Item("Log/Item_DB_Test");
    }

    private void ParsingToCsvFile_Item(string path)
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
        bool dropPossible = true;
        switch(boolean)
        {
            case "TRUE":
                dropPossible = true;
                break;
            case "FALSE":
                dropPossible = false;
                break;
        }
        float dropPercent = float.Parse(percent);
        itemBox.GetComponent<ItemBox>().SetDropTable(itemType,dropPossible, dropPercent);

        

    }

}
#endif