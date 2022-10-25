using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultInfoCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI killText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI surviveText;
    
    private void Awake()
    {
        SetUI();
    }

    private void SetUI()
    {
        InGamePlayerData data = IngameDataManager.Instance.OwnerData;

        killText.text = data.Kill.ToString();
        damageText.text = data.Damage.ToString();
        surviveText.text = data.LiveTime.ToString();
    }


    public void ChanageToMainScene()
    {
        SceneChanger_Loading.Instance.ChangeScene("Lobby_Main");
    }
}
