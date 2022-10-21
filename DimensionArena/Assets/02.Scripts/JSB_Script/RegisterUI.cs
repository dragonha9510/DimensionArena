using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
            warningText.text = "´Ð³×ÀÓÀ» ÀÔ·ÂÇØ ÁÖ¼¼¿ä";
            warningPopUp.SetActive(true);
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

        // ½ºÆä¼È ¹®ÀÚ°¡ ÀÖ°Å³ª
        // ÇÑ±¹¾î ¶Ç´Â ¿µ¾î ¶Ç´Â ¼ýÀÚ°¡ ¾Æ¿¹ ¾ø°Å³ª

        if (isInSpecial || (!isInKorean && !isInEnglish && !isInNumber))
        {
            nickNameInputField.text = "";
            warningText.text = "Æ¯¼ö¹®ÀÚ ¹× °ø¹éÀº µé¾î°¥ ¼ö ¾ø½À´Ï´Ù";
            warningPopUp.SetActive(true);
        }
        else
            GoToGameStartScene();
    }
    public void ActiveFalseWarningPopUp()
    {
        warningPopUp.SetActive(false);
    }    
    public void ActiveFalseNickCheckPopUp()
    {
        nickCheckPopUp.SetActive(false);
    }
    public void GoToGameStartScene()
    {
        SceneManager.LoadScene("GameStartScene");
        FirebaseDB_Manager.Instance.PlayerNickName = nickNameInputField.text;
    }
}
