using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace ManagerSpace
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


    public class ManagerMediator : MonoBehaviour
    {
        
        [SerializeField] SerializableDictionary<MANAGER_TYPE, GameObject> managerDic;
        private bool isAllManagerActive;
        public bool IsAllManagerActive => isAllManagerActive;


      

        private void Start()
        {

            foreach (var type in managerDic)
            {
                type.Value.SetActive(true);
            }

            isAllManagerActive = true;
        }

        IEnumerator DelayActiveManager()
        {
            for (int i = (int)MANAGER_TYPE.EFFECT; i < (int)MANAGER_TYPE.INGAME_DATA; ++i)
            {
                GameObject obj;

                if (managerDic.TryGetValue((MANAGER_TYPE)i, out obj))
                {
                    if(!obj)
                    {
                        Debug.LogError("할당되지 않은 매니저 : " + (MANAGER_TYPE)i + "가 있습니다.");
                        Application.Quit();
                    }

                    obj.SetActive(true);


                }
           
            }

            if (PhotonNetwork.IsConnected)
            {
                while (PhotonNetwork.CurrentRoom.PlayerCount
               != PlayerInfoManager.Instance.PlayerObjectArr.Length)
                {
                    yield return null;
                }

                for (int i = (int)MANAGER_TYPE.INGAME_DATA; i < (int)MANAGER_TYPE.MANAGER_TYPE_END; ++i)
                {
                    GameObject obj;
                    if (managerDic.TryGetValue((MANAGER_TYPE)i, out obj))
                    {
                        if (!obj)
                        {
                            Debug.LogError("할당되지 않은 매니저 : " + (MANAGER_TYPE)i + "가 있습니다.");
                            Application.Quit();
                        }
                        obj.SetActive(true);
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
                        if (!obj)
                        {
                            Debug.LogError("할당되지 않은 매니저 : " + (MANAGER_TYPE)i + "가 있습니다.");
                            Application.Quit();
                        }

                        obj.SetActive(true);
                    }
                }
            }
        }

    }




}


