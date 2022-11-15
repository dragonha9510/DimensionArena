using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialEvent : MonoBehaviour
{
    public struct EventType
    {
        public int idx;
        public string dialogue;
        public bool isCondition;        
    }

    List<Dictionary<string, object>> dialogue = new List<Dictionary<string, object>>();
    [SerializeField] SerializableDictionary<int, string> dicDialogue = new SerializableDictionary<int, string>();
    [SerializeField] TextMeshProUGUI curText;
    [SerializeField] CanvasGroup touchBlockGroup;

    [SerializeField] List<BaseEvent> listEvent = new List<BaseEvent>();
    BaseEvent curEvent;
    public SerializableDictionary<int, string> DicDialogue => dicDialogue;

    void Awake()
    {
        dialogue = CSVReader.Read("Log/Tutorial");
    }


    private void Update()
    {
        if (listEvent.Count < 1)
            return;

        if (!listEvent[0].CheckEventState())
            return;

        curText.text = listEvent[0].EventSuccesed();
        listEvent.RemoveAt(0);

    }


    public void TouchBlockOnOff(bool isOn)
    {
        touchBlockGroup.blocksRaycasts = isOn;
        touchBlockGroup.alpha = touchBlockGroup.blocksRaycasts ? 1.0f : 0.0f;
    }



   
}
