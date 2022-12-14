using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using System.Text.RegularExpressions;

public class RegisterUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nickNameInputField;

    [SerializeField]
    private GameObject warningPopUp;
    [SerializeField]
    private TextMeshProUGUI warningText;
    [SerializeField]
    private GameObject nickCheckPopUp;
    [SerializeField]
    private TextMeshProUGUI checkingNameText;
    [SerializeField]
    private GameObject welcomeObj;
    [SerializeField]
    private TextMeshProUGUI welcomeText;
    [SerializeField]
    private GameObject credit;
    private void Start()
    {

        DOTween.SetTweensCapacity(4000          , 0);
        StartCoroutine("DataUpdateCheck");
    }

    IEnumerator DataUpdateCheck()
    {
        while(true)
        {
            if (FirebaseDB_Manager.Instance.RefreshCount < 1)
                yield return null;
            else
            {
                if (FirebaseDB_Manager.Instance.RegisterDataBase())
                {
                    welcomeObj.SetActive(true);
                    welcomeText.text = "환영합니다 ";
                    welcomeText.text += FirebaseDB_Manager.Instance.PlayerNickName;
                    SceneManager.LoadScene("GameStartScene");
                }
                yield break;
            }
        }
        
    }
    public void CreditOn()
    {
        credit.SetActive(true);
    }
    public void CreditOff()
    {
        credit.SetActive(false);
    }
    public void NickNameCheck()
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");   
        string text = nickNameInputField.text;
        
        // 공백 체크
        if("" == text)
        {
            ActiveWarningPopUp("닉네임을 입력해 주세요");
            return;
        }

        if(FirebaseDB_Manager.Instance.NameOverlapCheck(text))
        {
            ActiveWarningPopUp("이미 등록된 이름이 있습니다");
            return;
        }

        Regex korRegex = new Regex(@"[ㄱ-ㅎ가-힣]");
        bool isInKorean = korRegex.IsMatch(text);
        Regex engRegex = new Regex(@"[a-zA-Z]");
        bool isInEnglish = engRegex.IsMatch(text);
        Regex numRegex = new Regex(@"[0-9]");
        bool isInNumber = numRegex.IsMatch(text);
        Regex specialRegex = new Regex(@"[~!@\#$%^&*\()\=+|\\/:;?""<>']");
        bool isInSpecial = specialRegex.IsMatch(text);
        Regex spaceRegex = new Regex(@"\s");
        bool isInSpace = spaceRegex.IsMatch(text);

        // 스페셜 문자가 있거나
        // 한국어 또는 영어 또는 숫자가 아예 없거나

        if (isInSpecial || isInSpace || (!isInKorean && !isInEnglish && !isInNumber))
        {
            ActiveWarningPopUp("특수문자 및 공백은 들어갈 수 없습니다");
        }
        else
            ActiveTrueNickCheckPopUp();
    }
    private void ActiveWarningPopUp(string infotext)
    {
        nickNameInputField.text = "";
        warningText.text = infotext;
        warningPopUp.SetActive(true);
    }
    public void ActiveFalseWarningPopUp()
    {
        SoundManager.Instance.PlaySFXOneShot("CancelEffect");
        warningPopUp.SetActive(false);
    }    
    public void ActiveTrueNickCheckPopUp()
    {
        checkingNameText.text = "이름을 \"" + nickNameInputField.text + "\" 로 하시겠습니까?";
        nickCheckPopUp.SetActive(true);
    }
    public void ActiveFalseNickCheckPopUp()
    {
        SoundManager.Instance.PlaySFXOneShot("CancelEffect");
        nickCheckPopUp.SetActive(false);
    }
    public void GoToGameStartScene()
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");
        FirebaseDB_Manager.Instance.RegisterNewPlayer(nickNameInputField.text);
        SceneManager.LoadScene("Tutorial");
    }
}
