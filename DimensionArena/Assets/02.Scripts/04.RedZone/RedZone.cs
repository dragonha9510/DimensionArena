using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal; // For Decal


using Photon.Pun;


public class RedZone : MonoBehaviourPun
{
    [SerializeField] private Transform innerBorder;
    [SerializeField] private MagneticField magneticField;
    private SafeZone safezone;

    [SerializeField] private float startDelay = 10.0f;
    [SerializeField] private float positionDelay = 10.0f;
    [SerializeField] private float prepareDelay = 10.0f;

    private bool isPreparing;
    private Vector3 prepareSpeed;

    private bool isReady;

    [SerializeField] private GameObject Missile;
    [SerializeField] private GameObject ItemMissile;
    [SerializeField] private float shotDelay;
    [SerializeField] private Vector2Int MinMaxmissileCntValue = new Vector2Int(0, 35);
    [SerializeField] private int lastMissileCnt = 10;
    private int missileCnt;
    private GameObject lastMissile;
    private int curMissileCnt = 0;
    private DelayMachine delayShot = new DelayMachine();

    [SerializeField] private GameObject Redzone;

    private DelayMachine delayPositioning = new DelayMachine();

    [SerializeField, Tooltip("TRUE means use Time, if Check false which will use repeatCnt")]
    private bool isDestroyWithTime;

    [SerializeField] private int repeatCnt = 1;
    private int curRepeatCnt = 0;
    [SerializeField] private float destroyTime = 120;
    private float curDestroyTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.OfflineMode && !PhotonNetwork.IsMasterClient)
            return;
        delayPositioning.DelayStart(startDelay);
        innerBorder.localScale = new Vector3(0, 2, 0);

        if(PhotonNetwork.OfflineMode)
            Redzone.gameObject.SetActive(false);
        else
            photonView.RPC("RedzoneActiveSetting", RpcTarget.All , false);

        missileCnt = Random.Range(MinMaxmissileCntValue.x, MinMaxmissileCntValue.y);
    }
    [PunRPC]
    private void RedzoneActiveSetting(bool state)
    {
        GameObject obj = this.transform.Find("RedZone").gameObject;
        obj.SetActive(state);
        //this.transform.Find("RedZone").gameObject.SetActive(state);
        //GameObject obj = GameObject.Find("RedZone");
        //Redzone.gameObject.SetActive(state);
    }
    // Update is called once per frame
    void Update()
    {
        if(!PhotonNetwork.OfflineMode)
        { 
            if (!PhotonNetwork.IsMasterClient)
                return;
        }

        curDestroyTime += Time.deltaTime;

        Positioning();
        Prepare();
        Shot();
    }

    private void Shot()
    {
        if (!isReady)
            return;

        if(delayShot.IsReady())
        {
            Vector3 pos = transform.position + (Random.insideUnitSphere * transform.localScale.x * 0.5f);
            pos.y = Missile.transform.position.y;

            if (PhotonNetwork.OfflineMode)
                lastMissile = Instantiate(Missile, pos, Quaternion.identity);
            else
                lastMissile = PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER + "Missile", pos, Quaternion.identity);
            delayShot.DelayReset();
            ++curMissileCnt;

            if (curMissileCnt >= missileCnt)
            {
                for (int i = 0; i < lastMissileCnt; ++i)
                {
                    pos = transform.position + (Random.insideUnitSphere * transform.localScale.x * 0.5f);
                    pos.y = Missile.transform.position.y;
                    if (PhotonNetwork.OfflineMode)
                        lastMissile = Instantiate(ItemMissile, pos, Quaternion.identity);
                    else    
                        lastMissile = PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER + "Item_Missile", pos, Quaternion.identity);
                }

                delayShot.DelayEnd(); 
            }
        }

        if (lastMissile == null && curMissileCnt >= missileCnt)
        {
            ++curRepeatCnt;
            delayPositioning.DelayStart(positionDelay);

            if (PhotonNetwork.OfflineMode)
            {
                innerBorder.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                innerBorder.localScale = new Vector3(0, 2, 0);
                Redzone.gameObject.SetActive(false);
            }
            else
            {
                photonView.RPC("InBorderScaleSetting", RpcTarget.All, new Vector3(0, 2, 0));
                photonView.RPC("RedzoneActiveSetting", RpcTarget.All, false);
            }
            curMissileCnt = 0;
            missileCnt = Random.Range(MinMaxmissileCntValue.x, MinMaxmissileCntValue.y);


            if (!isDestroyWithTime && curRepeatCnt >= repeatCnt)
                Destroy(this.gameObject);
            else if (isDestroyWithTime && curDestroyTime >= destroyTime)
                Destroy(this.gameObject);
        }
    }

    [PunRPC]
    private void InBorderScaleSetting(Vector3 scale)
    {
        innerBorder.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
       
        innerBorder.localScale = scale;
    }
    void Positioning()
    {
        if (isPreparing)
            return;

        if (delayPositioning.IsReady())
        {
            if (magneticField != null)
                safezone = magneticField.GetSafe;
            else
                safezone = new SafeZone();

            if (PhotonNetwork.OfflineMode)
            {
                transform.position = new Vector3(Random.Range(safezone.left, safezone.right), 0, Random.Range(safezone.top, safezone.bottom));
                Redzone.gameObject.SetActive(true);
            }
            else
            {
                Vector3 newPos = new Vector3(Random.Range(safezone.left, safezone.right), 0, Random.Range(safezone.top, safezone.bottom));
                photonView.RPC("RedZonePositioningOnServer", RpcTarget.All, newPos);
                photonView.RPC("RedzoneActiveSetting", RpcTarget.All, true);
            }
            delayPositioning.DelayEnd();
            isPreparing = true;
            prepareSpeed = new Vector3(1 / prepareDelay, 0, 1 / prepareDelay);
        }
    }
    
    [PunRPC]
    void RedZonePositioningOnServer(Vector3 pos)
    {
        Debug.Log("RedZonePositioningOnServer");
        this.transform.position = pos;
        //this.transform.position = new Vector3(0, 0, 0);
    }
    void Prepare()
    {
        if (!isPreparing)
            return;

        Vector3 scale = innerBorder.localScale + prepareSpeed * Time.deltaTime;

        if(PhotonNetwork.OfflineMode)
            innerBorder.localScale = scale;
        else
            photonView.RPC("InBorderScaleSetting", RpcTarget.All, scale);

        if (innerBorder.localScale.x >= 1)
        {
            isReady = true;
            isPreparing = false;
            delayShot.DelayStart(shotDelay);
            if(PhotonNetwork.OfflineMode)
                innerBorder.localScale = new Vector3(1, 2f, 1);
            else
                photonView.RPC("InBorderScaleSetting", RpcTarget.All, new Vector3(1, 2f, 1));
        }
    }
}
