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
            warningText.text = "�г����� �Է��� �ּ���";
            warningPopUp.SetActive(true);
            return;
        }
        
        Regex korRegex = new Regex(@"[��-����-�R]");
        bool isInKorean = korRegex.IsMatch(text);
        Regex engRegex = new Regex(@"[a-zA-Z]");
        bool isInEnglish = engRegex.IsMatch(text);
        Regex numRegex = new Regex(@"[0-9]");
        bool isInNumber = numRegex.IsMatch(text);
        Regex specialRegex = new Regex(@"[~!@\#$%^&*\()\=+|\\/:;?""<>']");
        bool isInSpecial = specialRegex.IsMatch(text);

        // ����� ���ڰ� �ְų�
        // �ѱ��� �Ǵ� ���� �Ǵ� ���ڰ� �ƿ� ���ų�

        if (isInSpecial || (!isInKorean && !isInEnglish && !isInNumber))
        {
            nickNameInputField.text = "";
            warningText.text = "Ư������ �� ������ �� �� �����ϴ�";
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
