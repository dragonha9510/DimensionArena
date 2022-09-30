using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParshing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Resources 에 존재하는 csv 파일명을 적어야 한다.
        List<Dictionary<string, object>> data_Dialog = CSVReader.Read("Item_DB");
        // [i]["name"] 에서 "name" 은 행 부분이며 행 기준으로 읽어 오겠다는 뜻 이다.
        for (int i = 0; i < data_Dialog.Count; ++i)
        {
            if (data_Dialog[i]["call"].ToString() == "#")
                continue;
            Debug.Log(data_Dialog[i]["name"].ToString());
        }
    }
}
