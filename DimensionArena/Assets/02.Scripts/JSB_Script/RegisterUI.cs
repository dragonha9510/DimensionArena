using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    

    public void NickNameCheck()
    {
        
        string text = nickNameInputField.text;
        
        if("" == text)
        {
            warningText.text = "닉네임을 입력해 주세요";
            warningPopUp.SetActive(true);
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

        // 스페셜 문자가 있거나
        // 한국어 또는 영어 또는 숫자가 아예 없거나

        if (isInSpecial || (!isInKorean && !isInEnglish && !isInNumber))
        {
            nickNameInputField.text = "";
            warningText.text = "특수문자 및 공백은 들어갈 수 없습니다";
            warningPopUp.SetActive(true);
        }
    }
    public void ActiveFalseWarningPopUp()
    {
        warningPopUp.SetActive(false);
    }    
    public void ActiveFalseNickCheckPopUp()
    {
        nickCheckPopUp.SetActive(false);
    }

}
