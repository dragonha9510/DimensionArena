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
                    GameObject inGameUIManager = GameObject.FindObjectOfType<InGameUIManager>().gameObject;

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
            public DeadEvent(string killerName, string victimName)
            {
                this.killerName = killerName;
                this.victimName = victimName;

                PlayerInfo info;
                if (PlayerInfoManager.Instance.DicPlayerInfo.TryGetValue(killerName, out info))
                    killerType = info.Type;
                else
                {
                    killerType = killerName.Equals("RedZone") ? UNITTYPE.RedZone : UNITTYPE.Magnetic;
                    this.killerName = killerName.Equals("RedZone") ? "레드존" : "자기장";
                }

                PlayerInfoManager.Instance.DicPlayerInfo.TryGetValue(victimName, out info);
                victimType = info.Type;
                
            }


            private string killerName;
            private string victimName;
            private UNITTYPE killerType;
            private UNITTYPE victimType;

            public string KillerName => killerName;
            public string VictimName => victimName;
            public UNITTYPE KillerType => killerType;
            public UNITTYPE VictimType => victimType;


        }

        [Header("Inform/Setting Parameter")]
        [SerializeField] private float announceTime;
        [SerializeField] private int countTime;

        [Range(0.001f, 0.005f)]
        [SerializeField] private float infoMoveSmoothNess;

        [Header("StartUI")]
        [SerializeField] private GameObject GameStartGroup;
        [SerializeField] private TextMeshProUGUI objectiveText;
        [SerializeField] private Image dimensionArenaLogo;
        [SerializeField] private TextMeshProUGUI[] surviverText;

        [Header("기획/StartAnimation")]
        [SerializeField] private float textSmallerDelay;
        [SerializeField] private float logoGrowDelay;
        private float totalStartDelay;

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
            mode = GameManager.Instance == null ? GAMEMODE.Survival : GameManager.Instance.GameMode;
            ListDeadEv = new List<DeadEvent>();
            isInfromEnd = true;

            switch (mode)
            {
                case GAMEMODE.Survival:
                    foreach(var info in PlayerInfoManager.Instance.DicPlayerInfo.Values)
                    {
                        info.EDeadPlayer += InformDeadPlayer;

                        if (info.ID.Equals(PhotonNetwork.NickName))
                            info.EDisActivePlayer += ResutUIOn;
                    }
                    break;
                case GAMEMODE.FreeForAll:
                    break;
            }
        }
        private void SetGameStart()
        {
            GAMEMODE mode;
            mode = GameManager.Instance == null ? GAMEMODE.Survival : GameManager.Instance.GameMode;

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
                    dynamicContent = PlayerInfoManager.Instance.DicPlayer.Count;
                    dynamicText.text = dynamicContent.ToString();
                    break;
                case GAMEMODE.FreeForAll:
                    //개발 예정
                    break;
            }

            foreach (var text in surviverText)
            {
                text.DOFade(1.0f, 1.0f);
            }

            touchCanvas.DOFade(1.0f, 0.5f);
            yield return new WaitForSeconds(0.5f);

            touchCanvas.interactable = true;
            touchCanvas.blocksRaycasts = true;
        }

        /// ===========================


        /// ===========================
        /// Eveent method Region
        /// >>>>>>>>>>>>>>>>>>>>>>>>>>


        Coroutine informCoroutine = null;
        private void InformDeadPlayer(string killerId, string victimId)
        {
            //Insert To DeadEvList
            ListDeadEv.Add(new DeadEvent(killerId, victimId));


            //만약, 실행하는 코루틴이 이번의 dead event도 처리했다면 새 코루틴 시작이 들어가지않는다.
            if (isInfromEnd
                && ListDeadEv.Count > 0)
            {
                if(informCoroutine != null)
                    StopCoroutine(InformDeadCoroutine());

                informCoroutine = StartCoroutine(InformDeadCoroutine());
            }
        }


        IEnumerator InformDeadCoroutine()
        {
            isInfromEnd = false;

            //새로운 이벤트가 들어온 것을 확인 했을때
            while (ListDeadEv.Count > 0)
            {
                informCanvas.alpha = 0.0f;
                dynamicContent -= 1;
                dynamicText.text = ((int)dynamicContent).ToString();
                //SetThumbnail
                SelectThumbnail(killerImage, ListDeadEv[0].KillerType);
                SelectThumbnail(victimImage, ListDeadEv[0].VictimType);

                //SetNickName
                killerNickName.text = ListDeadEv[0].KillerName;
                victimNickName.text = ListDeadEv[0].VictimName;

                informCanvas.gameObject.SetActive(true);
                informCanvas.DOFade(1.0f, fadeTime);
                yield return new WaitForSeconds(InformTime);
                informCanvas.DOFade(0.0f, fadeTime * 0.5f);
                yield return new WaitForSeconds(fadeTime * 0.5f);
                ListDeadEv.RemoveAt(0);
            }
            isInfromEnd = true;

        }


        void SelectThumbnail(Image image, UNITTYPE type)
        {
            switch (type)
            {
                case UNITTYPE.Aura:
                    image.sprite = CharacterThumbnail[(int)UNITTYPE.Aura];
                    break;
                case UNITTYPE.Ravagebell:
                    image.sprite = CharacterThumbnail[(int)UNITTYPE.Ravagebell];
                    break;
                case UNITTYPE.Joohyeok:
                    image.sprite = CharacterThumbnail[(int)UNITTYPE.Joohyeok];
                    break;
                case UNITTYPE.Secilia:
                    image.sprite = CharacterThumbnail[(int)UNITTYPE.Secilia];
                    break;
                case UNITTYPE.Magnetic:
                    image.sprite = CharacterThumbnail[(int)UNITTYPE.Magnetic];
                    break;
                case UNITTYPE.RedZone:
                    image.sprite = CharacterThumbnail[(int)UNITTYPE.RedZone];
                    break;
                case UNITTYPE.Securitas:
                    image.sprite = CharacterThumbnail[(int)UNITTYPE.Securitas];
                    break;
            }
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

        public void ResutUIOn()
        {

            winLoseText.text = IngameDataManager.Instance.OwnerData.Rank.Equals(1) ? "You Win!" : "You Die";
            GameEndGroup.SetActive(true);
            CanvasGroup group = GameEndGroup.GetComponent<CanvasGroup>();
            StartCoroutine(CanvasAlphaOn(group));
        }


        [PunRPC]
        public void WinUIOn()
        {
            winLoseText.text = "You Win!";
            GameEndGroup.SetActive(true);
            CanvasGroup group = GameEndGroup.GetComponent<CanvasGroup>();
            StartCoroutine(CanvasAlphaOn(group));
        }

        public void LoadResultScene()
        {
            GameManager.Instance.GameEnd();
            SceneManager.LoadScene("Result");        
        }
    }
}