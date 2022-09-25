using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RedZone : MonoBehaviour
{
    [SerializeField] private DecalProjector proj;
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
    private int missileCnt;
    private GameObject lastMissile;
    private int curMissileCnt = 0;
    private DelayMachine delayShot = new DelayMachine();

    [SerializeField] private GameObject Redzone;

    private DelayMachine delayPositioning = new DelayMachine();

    // Start is called before the first frame update
    void Start()
    {
        delayPositioning.DelayStart(startDelay);
        proj.size = new Vector3(0, 0, proj.size.z);
        Redzone.gameObject.SetActive(false);
        missileCnt = Random.Range(MinMaxmissileCntValue.x, MinMaxmissileCntValue.y);
    }

    // Update is called once per frame
    void Update()
    {
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
            Vector3 pos = transform.position + (Random.insideUnitSphere * 5f);
            pos.y = 10;
            lastMissile = Instantiate(Missile, pos, Quaternion.identity);
            delayShot.DelayReset();
            ++curMissileCnt;

            if(curMissileCnt >= missileCnt)
                delayShot.DelayEnd();
        }

        if(lastMissile == null && curMissileCnt >= missileCnt)
        {
            delayPositioning.DelayStart(positionDelay);
            proj.size = new Vector3(0, 0, proj.size.z);
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
            safezone = magneticField.GetSafe;
            transform.position = new Vector3(Random.Range(safezone.left, safezone.right), 0, Random.Range(safezone.top, safezone.bottom));

            Redzone.gameObject.SetActive(true);
            delayPositioning.DelayEnd();
            isPreparing = true;
            prepareSpeed = new Vector3(transform.localScale.x / prepareDelay, transform.localScale.z / prepareDelay, 0);
        }
    }
    
    void Prepare()
    {
        if (!isPreparing)
            return;

        proj.size += prepareSpeed * Time.deltaTime;

        if (proj.size.x >= transform.localScale.x)
        {
            isReady = true;
            isPreparing = false;
            delayShot.DelayStart(shotDelay);
            proj.size = new Vector3(transform.localScale.x, transform.localScale.z, 2.5f);
        }
    }
}
