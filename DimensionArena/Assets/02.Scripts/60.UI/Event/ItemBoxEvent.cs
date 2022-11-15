using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxEvent : BaseEvent
{
    [SerializeField] ItemBox[] item;
    [SerializeField] GameObject[] itemPrefab;

    public override bool CheckEventState()
    {
        for(int i = 0; i < item.Length; ++i)
        {
            if (item[i].Health <= 0)
                return true;

        }
        return false;
    }

    public override void EventSuccesed()
    {
        for(int i = 0; i < itemPrefab.Length; ++i)
            if(item[i] == null)
                itemPrefab[i].SetActive(true);

        base.EventSuccesed();
    }
}
