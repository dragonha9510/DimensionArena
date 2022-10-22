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
    [SerializeField]
    private GameObject welcomeObj;
    [SerializeField]
    private TextMeshProUGUI welcomeText;

    private void Start()
    {
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
                    welcomeText.text = "ȯ���մϴ� ";
                    welcomeText.text += FirebaseDB_Manager.Instance.PlayerNickName;
                    SceneManager.LoadScene("GameStartScene");
                }
                yield break;
            }
        }
        
    }

    public void NickNameCheck()
    {
        
        string text = nickNameInputField.text;
        
        // ���� üũ
        if("" == text)
        {
            ActiveWarningPopUp("�г����� �Է��� �ּ���");
            return;
        }

        if(FirebaseDB_Manager.Instance.NameOverlapCheck(text))
        {
            ActiveWarningPopUp("�̹� ��ϵ� �̸��� �ֽ��ϴ�");
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
            ActiveWarningPopUp("Ư������ �� ������ �� �� �����ϴ�");
        }
        else
            GoToGameStartScene();
    }
    private void ActiveWarningPopUp(string infotext)
    {
        nickNameInputField.text = "";
        warningText.text = infotext;
        warningPopUp.SetActive(true);
    }
    public void ActiveFalseWarningPopUp()
    {
        warningPopUp.SetActive(false);
    }    
    public void ActiveTrueNickCheckPopUp()
    {
        checkingNameText.text = "�̸��� \"" + nickNameInputField.text + "\" �� �Ͻðڽ��ϱ�?";
        nickCheckPopUp.SetActive(true);
    }
    public void ActiveFalseNickCheckPopUp()
    {
        nickCheckPopUp.SetActive(false);
    }
    public void GoToGameStartScene()
    {
        SceneManager.LoadScene("GameStartScene");
        FirebaseDB_Manager.Instance.RegisterNewPlayer(checkingNameText.text);
    }
}
