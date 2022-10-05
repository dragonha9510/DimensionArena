using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum CLIENTOBJ
{
    // 서버와 관련되지 않은 것
    CLIENTOBJ_CLOUDEFFECT,

}
public enum SERVEROBJ
{
    // 서버와 관련되지 않은 것
    
    SERVEROBJ_MISSILE,
    SERVEROBJ_ITEMMISSILE,
}

public class ObjectPool : MonoBehaviourPun
{

    [SerializeField]
    GameObject[] clientPrefabs;


    public static ObjectPool Instance = null;

    Dictionary<CLIENTOBJ, Queue<GameObject>> cliobjectPool = new Dictionary<CLIENTOBJ, Queue<GameObject>>();
    Dictionary<SERVEROBJ, Queue<GameObject>> serverobjectPool = new Dictionary<SERVEROBJ, Queue<GameObject>>();

    #region <<SingleTon>>


    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(gameObject);
    }
    #endregion

    private void Start()
    {
        Debug.Log(clientPrefabs.Length);
    }

    #region <<MakeObjectFunction>>
    private GameObject CreateObjectInResources(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, this.transform);
        obj.SetActive(false);
        return obj;
    }
    // On Server
    private GameObject CreateObjectInResources(string path)
    {
        GameObject obj = PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER + path, Vector3.zero, new Quaternion());
        // 이거 추후 문제 생길 수 있을 것 같음
        obj.transform.parent = this.transform;
        return obj;
    }
    #endregion


    public void MakePool(CLIENTOBJ objType, int makeCount)
    {
        /*// Eready setting this pool
        if (cliobjectPool[objType] != null)
            return;*/
        cliobjectPool.Add(objType, new Queue<GameObject>());
        for (int i = 0; i < makeCount; ++i)
        {
            GameObject newObj = new GameObject();
            switch (objType)
            {
                case CLIENTOBJ.CLIENTOBJ_CLOUDEFFECT:
                    newObj = CreateObjectInResources(clientPrefabs[(int)CLIENTOBJ.CLIENTOBJ_CLOUDEFFECT]);
                    break;
            }
            cliobjectPool[objType].Enqueue(newObj);
        }
    }

    public void MakePool(SERVEROBJ objType,int makeCount)
    {        
        /*// Eready setting this pool
        /*if (serverobjectPool[objType] != null)
            return;*/
        serverobjectPool.Add(objType, new Queue<GameObject>());
        for (int i = 0; i < makeCount; ++i)
        {
            GameObject newObj;
            switch(objType)
            {
                case SERVEROBJ.SERVEROBJ_MISSILE:
                    newObj = CreateObjectInResources("Projectile");
                    break;
                case SERVEROBJ.SERVEROBJ_ITEMMISSILE:
                    newObj = CreateObjectInResources("Missile");
                    break;
            }
            serverobjectPool[objType].Enqueue(new GameObject());
        }
    }
   


    public GameObject GetObjectInPool(CLIENTOBJ type)
    {
        GameObject obj = cliobjectPool[type].Dequeue();
        obj.SetActive(true);
        return obj;        
    }

    /*[System.Obsolete]
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
    }*/

}