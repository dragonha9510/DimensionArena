using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MapSelect : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] TextMeshProUGUI mapText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] MODE selctedMode;
    [SerializeField] GameObject prefabBtn;
    [SerializeField] List<Sprite> sprites; 

    private void Awake()
    {
        ParsingToCsvFile("Log/MapSelectorTest");
        content.GetChild(0).GetComponent<Button>().onClick.Invoke();
    }


    private void ParsingToCsvFile(string path)
    {
        
        List<Dictionary<string, object>> data_Map = CSVReader.Read(path);

        for (int i = 0; i < data_Map.Count; ++i)
        {
            if (data_Map[i]["Name"].ToString() == "#")
                continue;
            //Object Create
            CreateMapBtn(data_Map[i]["Name"].ToString(), 
                         data_Map[i]["Image"].ToString(),
                         data_Map[i]["Mode"].ToString(),
                         data_Map[i]["Description"].ToString());
        }
    }


    private void CreateMapBtn(string objName, string imgName, string mode, string description)
    {
        GameObject obj = Instantiate(prefabBtn,content);
        obj.name = objName;

        Button btn;
        TextMeshProUGUI btnName;
        Image           image;

        if(obj.transform.GetChild(0).
            TryGetComponent<TextMeshProUGUI>(out btnName))
        {
            btnName.text = objName;
        }

        if(obj.transform.GetChild(1).
            TryGetComponent<Image>(out image))
        {
            //sprite
            image.sprite = sprites.Find(str => str.name == imgName);
            image.SetNativeSize();
            if (image.rectTransform.sizeDelta.x >= 700.0f)
                image.rectTransform.sizeDelta = image.rectTransform.sizeDelta * 0.5f;
        }

        if(obj.TryGetComponent<Button>(out btn))
        {
            btn.onClick.AddListener(() => { ClickMapBtn(objName, mode, description); });
        }
    }

    public void ClickMapBtn(string name, string mode, string descrpition)
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");

        mapText.text = name;

        switch (mode)
        {
            case "MODE_SURVIVAL":
                selctedMode = MODE.MODE_SURVIVAL;
                break;
            case "MODE_TRAINING":
                Debug.Log("트레이닝 선택");
                selctedMode = MODE.MODE_FREEFALLALL;
                break;
            default:
                Debug.LogError("잘못된 값이 들어가있습니다. Map Select CSV File을 확인해주세요.");
                break;
        }

        descriptionText.text = descrpition;
    }


    public void SelectMode()
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");

        LobbyManagerRenewal.Instance.playMode = selctedMode;
        SceneManager.LoadScene("Lobby_Main");
    }
}
