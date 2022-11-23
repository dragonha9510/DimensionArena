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
            //시작 시 할당된 모는 
            foreach (var type in managerDic)
            {
                type.Value.SetActive(true);
            }

            isAllManagerActive = true;
        }
    }
}


