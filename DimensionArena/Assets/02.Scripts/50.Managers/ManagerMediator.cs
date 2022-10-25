using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ManagerMediator : MonoBehaviour
{
    public enum MANAGER_TYPE
    { 
        EFFECT,
        GAME,
        PLAYER_INFO,
        INGAME_DATA,
        INGAME_UI,
        MANAGER_TYPE_END
    }

    [SerializeField] SerializableDictionary<MANAGER_TYPE, GameObject> managerDic;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(DelayActiveManager());
    }

    IEnumerator DelayActiveManager()
    {
        for (int i = (int)MANAGER_TYPE.EFFECT; i < (int)MANAGER_TYPE.INGAME_DATA; ++i)
        {
            GameObject obj;
            if (managerDic.TryGetValue((MANAGER_TYPE)i, out obj))
            {
                obj.SetActive(true);
            }
            else
            {
                Debug.LogError("할당되지 않은 매니저 : " + (MANAGER_TYPE)i + "가 있습니다.");
            }
        }
    
        if (PhotonNetwork.IsConnected)
        {
            while (PhotonNetwork.CurrentRoom.PlayerCount
           != PlayerInfoManager.Instance.PlayerObjectArr.Length)
            {
                yield return null;
            }

            for(int i = (int)MANAGER_TYPE.INGAME_DATA; i < (int)MANAGER_TYPE.MANAGER_TYPE_END; ++i)
            {
                GameObject obj;
                if (managerDic.TryGetValue((MANAGER_TYPE)i, out obj))
                {
                    obj.SetActive(true);
                } 
                else
                {
                    Debug.LogError("할당되지 않은 매니저 : " + (MANAGER_TYPE)i + "가 있습니다.");
                }
            }
        }
        else
        {
            for (int i = (int)MANAGER_TYPE.PLAYER_INFO; i < (int)MANAGER_TYPE.MANAGER_TYPE_END; ++i)
            {
                GameObject obj;
                if (managerDic.TryGetValue((MANAGER_TYPE)i, out obj))
                {
                    obj.SetActive(true);
                }
                else
                {
                    Debug.LogError("할당되지 않은 매니저 : " + (MANAGER_TYPE)i + "가 있습니다.");
                }
            }
        }
    }

}
