using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class TutorialEvent : MonoBehaviour
{

    private static TutorialEvent instance;
    public static TutorialEvent Instance => instance;

    List<Dictionary<string, object>> dialogue = new List<Dictionary<string, object>>();
    [SerializeField] List<string> list_Dialogue = new List<string>();
    [SerializeField] TextMeshProUGUI curText;
    [SerializeField] CanvasGroup touchBlockGroup;
    [SerializeField] List<BaseEvent> listEvent = new List<BaseEvent>();
    int cnt = 0;
    float delayTime = 1.0f;
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        
        dialogue = CSVReader.Read("Log/Tutorial");
        for(int i = 0; i < dialogue.Count; ++i)
        {
            list_Dialogue.Add(dialogue[i]["string"].ToString());
        }

    }

    float curtime;
    private void Update()
    {
        curText.text = dialogue[cnt]["string"].ToString();

        if (listEvent.Count < 1)
            return;

        if (!listEvent[0].CheckEventState())
            return;

        cnt++;
        listEvent[0].EventSuccesed();
        listEvent.RemoveAt(0);

        if(listEvent.Any())
            listEvent[0].gameObject.SetActive(true);
    }


    public void TouchBlockOnOff(bool isOn)
    {
        Debug.Log(isOn);
        touchBlockGroup.blocksRaycasts = isOn;
        touchBlockGroup.alpha = touchBlockGroup.blocksRaycasts ? 0.6f : 0.0f;
    }



   
}
