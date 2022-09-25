using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool instance = null;
        Queue<GameObject> queObjectPool = null;
        [SerializeField]
        private GameObject cloudEffectObj;
        public ObjectPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ObjectPool();
                    return instance;
                }
                else
                    return instance;
            }
        }
        private void Awake()
        {
            Instance.Init(1000);
        }
        public GameObject makeCloud()
        {
            GameObject newObj = Instantiate(cloudEffectObj,this.transform);
            newObj.SetActive(false);
            return newObj;
        }
        public void Init(int makeCount)
        {
            for (int i = 0; i < makeCount; ++i)
            {
                queObjectPool.Enqueue(makeCloud());
            }
        }

        [System.Obsolete]
        public GameObject GetObj(Vector3 pos)
        {
            if(0 < Instance.queObjectPool.Count)
            {
                GameObject obj = queObjectPool.Dequeue();
                obj.transform.position = pos;
                obj.SetActive(true);
                return obj;
            }
            else
            {
                GameObject newObj = makeCloud();
                newObj.transform.position = pos;
                newObj.SetActive(true);
                return newObj;
            }
        }
        public void ReturnObj(GameObject reObj)
        {
            reObj.SetActive(false);
            reObj.transform.SetParent(this.transform);
            instance.queObjectPool.Enqueue(reObj);
        }

    }