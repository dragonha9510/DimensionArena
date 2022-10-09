using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
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

    private float dynamicContent;
    public float DynamicContent { get { return dynamicContent; } set { DynamicContent = value; } }

    [Header("EndUI")]
    [SerializeField] private GameObject GameEndGroup;
    [SerializeField] private TextMeshProUGUI winLoseText;


    [Header("Inform UI")]
    [SerializeField] private CanvasGroup        informCanvas;
    [SerializeField] private Image              killerImage;
    [SerializeField] private Image              victimImage;
    [SerializeField] private TextMeshProUGUI    killerNickName;
    [SerializeField] private TextMeshProUGUI    victimNickName;

    [Header("Inform UI Setting")]

    [Range(0.5f,1.5f)]
    [SerializeField] private float fadeTime;
    [SerializeField] private float InformTime;



    [Header("Extern Canvas")]
    [SerializeField] private CanvasGroup touchCanvas;


    [Header("Sprite Resources")]
    [SerializeField] Sprite[] CharacterThumbnail;

    private GAMEMODE mode;

    /// ===========================
    /// Start Method Region
    /// >>>>>>>>>>>>>>>>>>>>>>>>>>

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
                    PlayerInfoManager.Instance.PlayerInfoArr[i].EDeadPlayer += InformDeadPlayer;
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

    /// ===========================


    /// ===========================
    /// Eveent method Region
    /// >>>>>>>>>>>>>>>>>>>>>>>>>>


    private void InformDeadPlayer(CharacterType killerType, string killerId, CharacterType victimType, string victimId)
    {      
        //Inform Dead alpha
        StartCoroutine(InformDeadCoroutine(killerType,killerId, victimType,victimId));
    }


    IEnumerator InformDeadCoroutine(CharacterType killerType, string killerId, CharacterType victimType, string victimId)
    {
        informCanvas.alpha = 0.0f;

        dynamicContent -= 1;
        dynamicText.text = ((int)dynamicContent).ToString();

        //SetThumbnail
        SelectThumbnail(killerImage, killerType);
        SelectThumbnail(victimImage, victimType);

        //SetNickName
        killerNickName.text = killerId;
        victimNickName.text = victimId;

        informCanvas.gameObject.SetActive(true);
        informCanvas.DOFade(1.0f, fadeTime);
        yield return new WaitForSeconds(InformTime);
        informCanvas.DOFade(0.0f, fadeTime);

    }


    void SelectThumbnail(Image image, CharacterType type)
    {
        switch(type)
        {
            case CharacterType.Aura:
                image.sprite = CharacterThumbnail[(int)CharacterType.Aura];
                break;
            case CharacterType.Raebijibel:
                image.sprite = CharacterThumbnail[(int)CharacterType.Raebijibel];
                break;
            case CharacterType.Joohyeok:
                image.sprite = CharacterThumbnail[(int)CharacterType.Joohyeok];
                break;
            case CharacterType.Sesillia:
                image.sprite = CharacterThumbnail[(int)CharacterType.Sesillia];
                break;
        }
    }


}
