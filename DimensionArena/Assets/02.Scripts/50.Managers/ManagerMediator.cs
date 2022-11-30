using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;


namespace ManagerSpace
{
    public enum MANAGER_TYPE
    {
        EFFECT,
        GAME,
        PLAYER_INFO,
        TOUCH_CANVAS,
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

                if(type.Key.Equals(MANAGER_TYPE.TOUCH_CANVAS))
                {
                    type.Value.GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f);
                    type.Value.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }

            isAllManagerActive = true;
        }
    }
}


