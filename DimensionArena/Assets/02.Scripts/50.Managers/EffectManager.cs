using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PlayerSpace;

namespace ManagerSpace
{
    public class EffectManager : MonoBehaviour
    {
        private static EffectManager instance;
        public static EffectManager Instance
        {
            get
            {
                if (!instance)
                {
                    if (!(instance = FindObjectOfType<EffectManager>()))
                    {
                        GameObject obj = new GameObject("EffectManager");
                        instance = obj.AddComponent<EffectManager>();

                    }
                }
                return instance;
            }
        }


        [SerializeField] SerializableDictionary<string, GameObject> effectDictionary;

        public void CreateParticleEffectOnGameobject(Transform trans, string eventType)
        {
            switch (eventType)
            {
                case "Revive":
                    if (effectDictionary.ContainsKey("Revive"))
                        Instantiate(effectDictionary["Revive"], trans.position + (Vector3.up * 0.5f), trans.rotation, trans);
                    break;
                case "Dead":
                    if (effectDictionary.ContainsKey("Dead"))
                        Instantiate(effectDictionary["Dead"], trans.position + (Vector3.up * 0.5f), trans.rotation, trans);
                    break;
                // JSB
                case "ItemDrop":
                    if(effectDictionary.ContainsKey("ItemDrop"))
                        Instantiate(effectDictionary["ItemDrop"], trans.position + (Vector3.up * 0.5f), trans.rotation, trans);
                    break;               
            }
        }
    }

}
