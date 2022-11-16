using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ManagerSpace;

public class ResultCanvas : MonoBehaviour
{
    [SerializeField] GameObject infoCanvas;
    [SerializeField] Image rankImage;
    [SerializeField] Sprite[] rankResource;
    [SerializeField] TextMeshProUGUI rankText;

    private InGamePlayerData data;

    private void Awake()
    {
        data = IngameDataManager.Instance.OwnerData;
        SetUI();
    }


    private void SetUI()
    {
        FirebaseDB_Manager.Instance.SavePlayerResultData(IngameDataManager.Instance.OwnerData);
        if(data.Rank != 1)
            rankImage.sprite = rankResource[0];
        else
            rankImage.sprite = rankResource[1];

        switch (data.Rank)
        {
            case 1:
                rankText.text = "1st!";
                break;
            case 2:
                rankText.text = "2nd";
                break;
            case 3:
                rankText.text = "3rd";
                break;
            default:
                rankText.text = data.Rank + "th";
                break;    
        }
        
    }


    public void InfoCanvasActive()
    {
        infoCanvas.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
