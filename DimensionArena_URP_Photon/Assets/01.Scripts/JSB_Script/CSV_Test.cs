using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSV_Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Dictionary<string, object>> data_Dialog = CSVReader.Read("Item_DB");
        for(int i = 0; i < data_Dialog.Count;++i)
        {
            Debug.Log(data_Dialog[i]["name"].ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
