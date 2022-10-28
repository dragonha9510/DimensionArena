using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResultInfoCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rankText;
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

        rankText.text = data.Rank.ToString();
        killText.text = data.Kill.ToString();
        damageText.text = data.Damage.ToString();
        surviveText.text = data.LiveTime.ToString();
    }


    public void ChanageToMainScene()
    {
        IngameDataManager.Instance.DestroyManager();
        SceneChanger_Loading.Instance.ChangeScene("Lobby_Main");
    }
}
