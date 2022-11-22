using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ManagerSpace;

public class ResultInfoCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] TextMeshProUGUI killText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI surviveText;
    InGamePlayerData data;

    private void Awake()
    {
        SetUI();
    }

    private void SetUI()
    {
        data = IngameDataManager.Instance.OwnerData;

        data = new InGamePlayerData("ธÞที");
        data.kill = 12;
        data.damage = 2653;
        data.liveTime = 487;
        data.rank = 1;

        if (data.Rank == 1)
            rankText.text = data.Rank.ToString() + "st!";
        else if(data.Rank == 2)
            rankText.text = data.Rank.ToString() + "nd!";
        else if (data.Rank == 3)
            rankText.text = data.Rank.ToString() + "rd!";
        else
            rankText.text = data.Rank.ToString() + "th!";

        moveReady = true;
        cntActive = true;
        //killText.text = data.Kill.ToString();
        //damageText.text = data.Damage.ToString();
        //surviveText.text = data.LiveTime.ToString();
    }

    public void skipUI()
    {
        float time = 1;
        resetUpdate(ref time, ref level);
    }

    private bool cntActive;
    private int level;
    private int killCnt = 0;
    private float damageCnt = 0;
    private float liveTime = 0;

    private float curTime;
    [SerializeField, Range(0, 1)] private float speed = 0.5f;

    [SerializeField] private Transform[] objectMove;
    [SerializeField, Range(0.1f, 1)] private float objectMovespeed;
    bool moveReady;

    void ObjectMovement(int idx)
    {
        if (!moveReady)
            return;

        Vector3 pos = objectMove[idx].localPosition;
        pos.x += 443f * Time.deltaTime * (1f / objectMovespeed);

        if (pos.x >= 43)
        {
            pos.x = 43;
            objectMove[idx].localPosition = pos;
            moveReady = false;
        }
        else
            objectMove[idx].localPosition = pos;
    }

    private void Update()
    {
        if (!cntActive)
        {
            killText.text = data.Kill.ToString();
            damageText.text = data.Damage.ToString();
            surviveText.text = data.LiveTime.ToString();

            for(int i = 0; i < 3; ++i)
            {
                Vector3 pos = objectMove[i].localPosition;
                pos.x = 43;
                objectMove[i].localPosition = pos;
            }

            return;
        }

        if(!moveReady)
            curTime += Time.deltaTime * speed;
        else
            ObjectMovement(level);

        switch (level)
        {
            case 0:
                killCnt = (int)(data.Kill * curTime);
                killText.text = killCnt.ToString();
                break;
            case 1:
                killText.text = data.Kill.ToString();

                damageCnt = data.Damage * curTime;
                damageText.text = damageCnt.ToString();
                break;
            case 2:
                killText.text = data.Kill.ToString();
                damageText.text = data.Damage.ToString();

                liveTime = data.LiveTime * curTime;
                surviveText.text = liveTime.ToString();
                break;
        }

        for (int i = 0; i < level; ++i)
        {
            Vector3 pos = objectMove[i].localPosition;
            pos.x = 43f;
            objectMove[i].localPosition = pos;
        }

        resetUpdate(ref curTime, ref level);
    }

    void resetUpdate(ref float time, ref int l)
    {
        if(time >= 1)
        {
            time = 0;
            ++l;
            moveReady = true;

            if (l == 3)
            {
                moveReady = false;
                cntActive = false;
                l = 0;
            }
        }
    }

    public void ChanageToMainScene()
    {
        PhotonNetwork.LeaveRoom();
        IngameDataManager.Instance.DestroyManager();
        Destroy(IngameDataManager.Instance.gameObject);
        SceneChanger_Loading.Instance.ChangeScene("Lobby_Main");

    }
}
