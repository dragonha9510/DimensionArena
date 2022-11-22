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
            GameObject particle = null;
            switch (eventType)
            {
                case "Revive":
                    if (effectDictionary.ContainsKey("Revive"))
                        particle = Instantiate(effectDictionary["Revive"], trans.position + (Vector3.up * 0.5f), trans.rotation);
                    break;
                case "Dead":
                    if (effectDictionary.ContainsKey("Dead"))
                        particle =Instantiate(effectDictionary["Dead"], trans.position + (Vector3.up * 0.5f), trans.rotation);
                    break;
                // JSB
                case "ItemDrop":
                    if(effectDictionary.ContainsKey("ItemDrop"))
                        particle = Instantiate(effectDictionary["ItemDrop"], trans.position + (Vector3.up * 0.5f), trans.rotation);
                    break;               

            }

            Destroy(particle, 3.0f);
        }

        public void CreateParticleEffectOnGameobject(Vector3 pos,Quaternion rot, string eventType)
        {
            GameObject particle = null;
            switch (eventType)
            {
                case "ItemDrop":
                    if (effectDictionary.ContainsKey("ItemDrop"))
                        particle = Instantiate(effectDictionary["ItemDrop"], pos + (Vector3.up * 0.5f), rot);
                    break;
            }
            Destroy(particle, 3.0f);
        }
    }

}
