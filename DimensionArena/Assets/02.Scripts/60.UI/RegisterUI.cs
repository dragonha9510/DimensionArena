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
                    welcomeText.text = "È¯¿µÇÕ´Ï´Ù ";
                    welcomeText.text += FirebaseDB_Manager.Instance.PlayerNickName;
                    SceneManager.LoadScene("GameStartScene");
                }
                yield break;
            }
        }
        
    }

    public void NickNameCheck()
    {
        SoundManager.Instance.PlaySFXOneShot("ClickEffect");   
        string text = nickNameInputField.text;
        
        // °ø¹é Ã¼Å©
        if("" == text)
        {
            ActiveWarningPopUp("´Ð³×ÀÓÀ» ÀÔ·ÂÇØ ÁÖ¼¼¿ä");
            return;
        }

        if(FirebaseDB_Manager.Instance.NameOverlapCheck(text))
        {
            ActiveWarningPopUp("ÀÌ¹Ì µî·ÏµÈ ÀÌ¸§ÀÌ ÀÖ½À´Ï´Ù");
            return;
        }

        Regex korRegex = new Regex(@"[¤¡-¤¾°¡-ÆR]");
        bool isInKorean = korRegex.IsMatch(text);
        Regex engRegex = new Regex(@"[a-zA-Z]");
        bool isInEnglish = engRegex.IsMatch(text);
        Regex numRegex = new Regex(@"[0-9]");
        bool isInNumber = numRegex.IsMatch(text);
        Regex specialRegex = new Regex(@"[~!@\#$%^&*\()\=+|\\/:;?""<>']");
        bool isInSpecial = specialRegex.IsMatch(text);
        Regex spaceRegex = new Regex(@"\s");
        bool isInSpace = spaceRegex.IsMatch(text);

        // ½ºÆä¼È ¹®ÀÚ°¡ ÀÖ°Å³ª
        // ÇÑ±¹¾î ¶Ç´Â ¿µ¾î ¶Ç´Â ¼ýÀÚ°¡ ¾Æ¿¹ ¾ø°Å³ª

        if (isInSpecial || isInSpace || (!isInKorean && !isInEnglish && !isInNumber))
        {
            ActiveWarningPopUp("Æ¯¼ö¹®ÀÚ ¹× °ø¹éÀº µé¾î°¥ ¼ö ¾ø½À´Ï´Ù");
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
        checkingNameText.text = "ÀÌ¸§À» \"" + nickNameInputField.text + "\" ·Î ÇÏ½Ã°Ú½À´Ï±î?";
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
