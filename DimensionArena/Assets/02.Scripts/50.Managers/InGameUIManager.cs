using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InGameUIManager : MonoBehaviour
{
    [Header("Setting Parameter")]
    [SerializeField] private float announceTime;
    [SerializeField] private int   countTime;

    [Range(0.001f, 0.005f)]
    [SerializeField] private float infoMoveSmoothNess;

    [Header("StartUI")]
    [SerializeField] private GameObject GameStartGroup;
    [SerializeField] private TextMeshProUGUI objectiveText;

    [Header("InforMationUI")]
    [SerializeField] private RectTransform[] infoTransform;
    private Dictionary<string, RectTransform>  DicInfoTransform;

    [SerializeField] private TextMeshProUGUI dynamicText;
    private object dynamicContent;
    public object DynamicContent { get { return dynamicText; } set { DynamicContent = value; } }

    [Header("EndUI")]
    [SerializeField] private GameObject GameEndGroup;
    [SerializeField] private TextMeshProUGUI winLoseText;

    [Header("Canvas")]
    [SerializeField] private CanvasGroup touchCanvas;

    private GAMEMODE mode;

    private void Start()
    {
        Initialize();
        StartCoroutine(StartUICoroutine());
    }


    private void Initialize()
    {    
        mode = GameManager.instance == null ? GAMEMODE.Survival : GameManager.instance.GameMode;

        switch (mode)
        {
            case GAMEMODE.Survival:
                for(int i = 0; i < PlayerInfoManager.Instance.PlayerInfoArr.Length; ++i)
                {
                    //PlayerInfoManager.Instance.PlayerInfoArr[i].EDeadPlayer += PlayerDead();
                }
                break;
            case GAMEMODE.FreeForAll:
                break;
        }


        DicInfoTransform = new Dictionary<string, RectTransform>();

        //gameInfo Image Transfrom Add
        for (int i = 0; i < infoTransform.Length; ++i)
        {
            DicInfoTransform.Add(infoTransform[i].gameObject.name, infoTransform[i]);
        }
    }

    public void PlayerDead()
    {
        //ui뛰우기

    }


    private IEnumerator StartUICoroutine()
    {
        //touch Canvas Setting
        touchCanvas.interactable = false;
        touchCanvas.alpha = 0;


        //Announce Objective Text
        SetGameStart();
        yield return new WaitForSeconds(announceTime);

        WaitForSeconds oneSeconds = new WaitForSeconds(1.0f);
        //Text Count Animation
        for(int i = countTime; i > 0 ; --i)
        {
            objectiveText.text = i.ToString();
            //Add Animation
            yield return oneSeconds;
        }

        GameStartGroup.SetActive(false);

        //Setting InfoUI
        StartCoroutine(InfoUICoroutine());

    }
    private void SetGameStart()
    {
        GAMEMODE mode;
        mode = GameManager.instance == null ? GAMEMODE.Survival : GameManager.instance.GameMode;

        switch (mode)
        {
            case GAMEMODE.Survival:
                objectiveText.text = "마지막까지\n살아남으세요!";
                break;
            case GAMEMODE.FreeForAll:
                objectiveText.text = "가장 많은\n점수를얻으세요!";
                break;
        }
    }


    private IEnumerator InfoUICoroutine()
    {
        switch(mode)
        {
            case GAMEMODE.Survival:
                dynamicContent = PlayerInfoManager.Instance.PlayerInfoArr.Length;
                dynamicText.text = dynamicContent.ToString();
                break;
            case GAMEMODE.FreeForAll:
                //개발 예정
                break;
        }


        for (int i = 0; i < 100; ++i)
        {
            DicInfoTransform["UpImage"].anchoredPosition += Vector2.down;
            DicInfoTransform["UnderImage"].anchoredPosition += Vector2.up;
            touchCanvas.alpha = i * 0.01f;
            yield return new WaitForSeconds(infoMoveSmoothNess);
        }

        touchCanvas.alpha = 1;
        touchCanvas.interactable = true;

    }



}
