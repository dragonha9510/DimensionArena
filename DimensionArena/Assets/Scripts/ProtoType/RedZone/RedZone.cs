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
    [SerializeField] private float shotDelay;
    [SerializeField] private Vector2Int MinMaxmissileCntValue = new Vector2Int(0, 35);
    [SerializeField] private int lastMissileCnt = 10;
    private int missileCnt;
    private GameObject lastMissile;
    private int curMissileCnt = 0;
    private DelayMachine delayShot = new DelayMachine();

    [SerializeField] private GameObject Redzone;

    private DelayMachine delayPositioning = new DelayMachine();

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        delayPositioning.DelayStart(startDelay);
        innerBorder.localScale = new Vector3(0, 2, 0);
        Redzone.gameObject.SetActive(false);
        missileCnt = Random.Range(MinMaxmissileCntValue.x, MinMaxmissileCntValue.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
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
            lastMissile = Instantiate(Missile, pos, Quaternion.identity);
            delayShot.DelayReset();
            ++curMissileCnt;

            if (curMissileCnt >= missileCnt)
            {
                for (int i = 0; i < lastMissileCnt; ++i)
                {
                    pos = transform.position + (Random.insideUnitSphere * transform.localScale.x * 0.5f);
                    pos.y = Missile.transform.position.y;
                    lastMissile = Instantiate(Missile, pos, Quaternion.identity);
                }

                delayShot.DelayEnd(); 
            }
        }

        if(lastMissile == null && curMissileCnt >= missileCnt)
        {
            delayPositioning.DelayStart(positionDelay);
            innerBorder.localScale = new Vector3(0, 2, 0);
            Redzone.gameObject.SetActive(false);
            curMissileCnt = 0;
            missileCnt = Random.Range(MinMaxmissileCntValue.x, MinMaxmissileCntValue.y);
        }
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

            transform.position = new Vector3(Random.Range(safezone.left, safezone.right), 0, Random.Range(safezone.top, safezone.bottom));

            Redzone.gameObject.SetActive(true);
            delayPositioning.DelayEnd();
            isPreparing = true;
            prepareSpeed = new Vector3(1 / prepareDelay, 0, 1 / prepareDelay);
        }
    }
    
    void Prepare()
    {
        if (!isPreparing)
            return;

        innerBorder.localScale += prepareSpeed * Time.deltaTime;

        if (innerBorder.localScale.x >= 1)
        {
            isReady = true;
            isPreparing = false;
            delayShot.DelayStart(shotDelay);
            innerBorder.localScale = new Vector3(1, 2f, 1);
        }
    }
}
