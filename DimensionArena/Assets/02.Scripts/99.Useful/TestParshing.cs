using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParshing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Resources �� �����ϴ� csv ���ϸ��� ����� �Ѵ�.
        List<Dictionary<string, object>> data_Dialog = CSVReader.Read("Item_DB");
        // [i]["name"] ���� "name" �� �� �κ��̸� �� �������� �о� ���ڴٴ� �� �̴�.
        for (int i = 0; i < data_Dialog.Count; ++i)
        {
            if (data_Dialog[i]["call"].ToString() == "#")
                continue;
            Debug.Log(data_Dialog[i]["name"].ToString());
        }
    }
}
