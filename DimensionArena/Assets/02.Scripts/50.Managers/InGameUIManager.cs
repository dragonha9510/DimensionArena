using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using ManagerSpace;


namespace ManagerSpace
{
    public class InGameUIManager : MonoBehaviour
    {
        private static InGameUIManager instance;

        public static InGameUIManager Instance
        {
            get
            {
                if (null == instance)
                {
                    GameObject inGameUIManager = GameObject.Find("InGameUIManager");

                    if (!inGameUIManager)
                    {
                        inGameUIManager = new GameObject("InGameUIManager");
                        inGameUIManager.AddComponent<PlayerInfoManager>();
                        inGameUIManager.AddComponent<PhotonView>();
                    }

                    instance = inGameUIManager.GetComponent<InGameUIManager>();
                }

                return instance;
            }
        }

        private struct DeadEvent
        {
            public DeadEvent(CharacterType killerType, string killerName,
                CharacterType victimType, string victimName)
            {
                this.killerName = killerName;
                this.victimName = victimName;
                this.killerType = killerType;
                this.victimType = victimType;
            }

            private string killerName;
            private string victimName;
            private CharacterType killerType;
            private CharacterType victimType;

            public string KillerName => killerName;
            public string VictimName => victimName;
            public CharacterType KillerType => killerType;
            public CharacterType VictimType => victimType;


        }

        [Header("Setting Parameter")]
        [SerializeField] private float announceTime;
        [SerializeField] private int countTime;

        [Range(0.001f, 0.005f)]
        [SerializeField] private float infoMoveSmoothNess;

        [Header("StartUI")]
        [SerializeField] private GameObject GameStartGroup;
        [SerializeField] private TextMeshProUGUI objectiveText;
        [SerializeField] private Image dimensionArenaLogo;

        [Header("기획/StartAnimation")]
        [SerializeField] private float textSmallerDelay;
        [SerializeField] private float logoGrowDelay;
        private float totalStartDelay;

        [Header("InforMationUI")]
        [SerializeField] private RectTransform[] infoTransform;
        private Dictionary<string, RectTransform> DicInfoTransform;

        [SerializeField] private TextMeshProUGUI dynamicText;

        private float dynamicContent;
        public float DynamicContent { get { return dynamicContent; } set { DynamicContent = value; } }

        [Header("EndUI")]
        [SerializeField] private GameObject GameEndGroup;
        [SerializeField] private TextMeshProUGUI winLoseText;


        [Header("Inform UI")]
        [SerializeField] private CanvasGroup informCanvas;
        [SerializeField] private Image killerImage;
        [SerializeField] private Image victimImage;
        [SerializeField] private TextMeshProUGUI killerNickName;
        [SerializeField] private TextMeshProUGUI victimNickName;

        [Header("기획/Inform UI Setting")]
        [Range(0.5f, 1.5f)]
        [SerializeField] private float fadeTime;
        [SerializeField] private float InformTime;

        [Header("Extern Canvas")]
        [SerializeField] private CanvasGroup touchCanvas;


        [Header("Sprite Resources")]
        [SerializeField] Sprite[] CharacterThumbnail;

        private GAMEMODE mode;

        private List<DeadEvent> ListDeadEv;
        bool isInfromEnd;

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
            ListDeadEv = new List<DeadEvent>();
            isInfromEnd = true;


            switch (mode)
            {
                case GAMEMODE.Survival:
                    for (int i = 0; i < PlayerInfoManager.Instance.PlayerInfoArr.Length; ++i)
                    {
                        PlayerInfoManager.Instance.PlayerInfoArr[i].EDeadPlayer += InformDeadPlayer;

                        if (PlayerInfoManager.Instance.PlayerObjectArr[i].name == PhotonNetwork.NickName)
                            PlayerInfoManager.Instance.PlayerInfoArr[i].EDisActivePlayer += DefeatUIOn;
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
            touchCanvas.blocksRaycasts = false;

            Prototype_TargetCamera camera = FindObjectOfType<Prototype_TargetCamera>();
            camera.FollowTargetAtGameStart();
            SetGameStart();
            //Announce Objective Text
            yield return new WaitForSeconds(DelayTime(announceTime));

            objectiveText.rectTransform.DOScale(Vector3.zero, textSmallerDelay);
            yield return new WaitForSeconds(DelayTime(textSmallerDelay));

            dimensionArenaLogo.transform.DOScale(Vector3.one, logoGrowDelay);
            yield return new WaitForSeconds(DelayTime(logoGrowDelay + 1.0f));


            GameStartGroup.SetActive(false);
            //Setting InfoUI
            StartCoroutine(InfoUICoroutine());

        }

        private float DelayTime(float delay)
        {
            totalStartDelay += delay;
            return delay;

        }

        private IEnumerator InfoUICoroutine()
        {

            switch (mode)
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
            touchCanvas.blocksRaycasts = true;
        }

        /// ===========================


        /// ===========================
        /// Eveent method Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>


        private void InformDeadPlayer(CharacterType killerType, string killerId, CharacterType victimType, string victimId)
        {
            //Insert To DeadEvList
            ListDeadEv.Add(new DeadEvent(killerType, killerId, victimType, victimId));


            //만약, 실행하는 코루틴이 이번의 dead event도 처리했다면 새 코루틴 시작이 들어가지않는다.
            if (isInfromEnd
                && ListDeadEv.Count > 0)
            {
                StopCoroutine(InformDeadCoroutine(killerType, killerId, victimType, victimId));
                StartCoroutine(InformDeadCoroutine(killerType, killerId, victimType, victimId));
            }
        }


        IEnumerator InformDeadCoroutine(CharacterType killerType, string killerId, CharacterType victimType, string victimId)
        {
            isInfromEnd = false;

            //새로운 이벤트가 들어온 것을 확인 했을때
            while (ListDeadEv.Count > 0)
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
                informCanvas.DOFade(0.0f, fadeTime * 0.5f);
                yield return new WaitForSeconds(fadeTime * 0.5f);
                ListDeadEv.RemoveAt(0);
            }
            isInfromEnd = true;

        }


        void SelectThumbnail(Image image, CharacterType type)
        {
            switch (type)
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

        public void DefeatUIOn()
        {
            GameEndGroup.SetActive(true);
            CanvasGroup group = GameEndGroup.GetComponent<CanvasGroup>();
            StartCoroutine(CanvasAlphaOn(group));
        }


        public void WinUIOn()
        {
            winLoseText.text = "You Win!";
            GameEndGroup.SetActive(true);
            CanvasGroup group = GameEndGroup.GetComponent<CanvasGroup>();
            StartCoroutine(CanvasAlphaOn(group));
        }

        IEnumerator CanvasAlphaOn(CanvasGroup group)
        {
            WaitForSeconds delay = new WaitForSeconds(0.01f);
            for (int i = 0; i < 100; ++i)
            {
                group.alpha += 0.01f;
                yield return delay;
            }

            group.alpha = 1.0f;
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        public void WatchTarget()
        {
            Debug.Log("너를 죽인애 쳐다보기");
        }


        public void LoadResultScene()
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Result");
        }
    }
}