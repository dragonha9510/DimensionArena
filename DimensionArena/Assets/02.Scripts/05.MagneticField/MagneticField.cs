using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public struct SafeZone
{

    public int phaseIndex;

    public float left;
    public float right;
    public float top;
    public float bottom;
    public float height { get { return top - bottom; } }
    public float width { get { return right - left; } }

}

public class MagneticField : MonoBehaviourPun
{
    private enum CLOUDTYPE
    {
        CLOUDTYPE_LEFT,
        CLOUDTYPE_RIGHT,
        CLOUDTYPE_TOP,
        CLOUDTYPE_BOTOTM
    }

    // 몇번에 거쳐서 축소가 될건지
    [SerializeField] int damageZoneTimeDivideCount = 5;
    // 쿨타임이 걸린다면 머무를 시간.
    [SerializeField] float divideStayTime = 0.5f;
    private float divideTime = 0.0f;
    // 자기장이 축소하지 않는 상태의 누적 시간
    private float inDivideAccumTime = 0.0f;
    // 최종적으로 줄어들 시간
    [SerializeField] float decreaseTime = 10.0f;

    [SerializeField]
    private float magneticFieldStartTime = 10.0f;
    private bool isReadyField = false;


    private float decreaseOneCircleActiveTime;


    [SerializeField] Vector2 magneticfieldScale;

    [SerializeField] Transform leftTopGround;
    [SerializeField] Transform leftBottomGround;
    [SerializeField] Transform rightTopGround;
    [SerializeField] Transform rightBottomGround;


    private SafeZone willDecreaseSafeZone = new SafeZone();
    public SafeZone GetSafe => willDecreaseSafeZone;

    private SafeZone[] calculatedSafeZones = null;

    private int nowPhaseIndex = 0;

    private float leftFrameCorrection;
    private float rightFrameCorrection;
    private float topFrameCorrection;
    private float bottomFrameCorrection;




    private List<GameObject> magneticfieldObj = new List<GameObject>();
    private Vector2 magneticfieldpos;

    private void Update()
    {
        if (calculatedSafeZones == null)
            return;
        else
            willDecreaseSafeZone = calculatedSafeZones[nowPhaseIndex];
    }
    
    private void SettingMagneticField()
    {
        // 확장 주기 시간 설정
        decreaseOneCircleActiveTime = decreaseTime / (float)damageZoneTimeDivideCount;

        GameObject cloud;
        float[] originalScale = new float[2];
        // 왼쪽 면
        cloud = PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER + "MagneticLeftRounge", this.transform.position, this.transform.rotation);


        // 포톤에 의해 변경이 된 부분
        SetEachOther(cloud, leftTopGround, leftBottomGround);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Left;
        originalScale[0] = ((rightTopGround.position.x - leftTopGround.position.x) / 2) * (leftTopGround.position.z - leftBottomGround.position.z);
        //cloud.GetComponent<MagneticCloudEffectCreator>().originalScale = originalScale[0];
        magneticfieldObj.Add(cloud);

        // 오른쪽 면
        cloud = PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER + "MagneticRightRounge", this.transform.position, this.transform.rotation);
        SetEachOther(cloud, rightTopGround, rightBottomGround);
        //cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Right;
        //cloud.GetComponent<MagneticCloudEffectCreator>().originalScale = originalScale[0];
        magneticfieldObj.Add(cloud);

        // 상단 면
        cloud = PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER + "MagneticTopRounge", this.transform.position, this.transform.rotation);
        SetEachOther(cloud, rightTopGround, leftTopGround);
        //cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Top;
        originalScale[1] = ((rightTopGround.position.z - rightBottomGround.position.z) / 2) * (rightTopGround.position.x - leftTopGround.position.x);
        cloud.GetComponent<MagneticCloudEffectCreator>().originalScale = originalScale[1];


        magneticfieldObj.Add(cloud);

        // 하단 면
        cloud = PhotonNetwork.Instantiate(PHOTONPATH.PHOTONPATH_PREFAPBFOLDER + "MagneticBottomRounge", this.transform.position, this.transform.rotation);
        SetEachOther(cloud, rightBottomGround, leftBottomGround);

        cloud.GetComponent<MagneticCloudEffectCreator>().originalScale = originalScale[1];


        magneticfieldObj.Add(cloud);
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SettingCloudType", RpcTarget.All);
            photonView.RPC("SettingOriginalScale", RpcTarget.All, originalScale[0], originalScale[1]);
            photonView.RPC("SettingPartnerCloud", RpcTarget.All);
        }

        calculatedSafeZones = new SafeZone[damageZoneTimeDivideCount + 1];

        SettingRandomPosition();
    }
    void Start()
    {
        // ObjectPool
        ObjectPool.Instance.MakePool(CLIENTOBJ.CLIENTOBJ_CLOUDEFFECT, 5000);
        //
        if (!PhotonNetwork.IsMasterClient)
            return;
    }

    [PunRPC]
    void SettingCloudType()
    {
        GameObject.Find("MagneticLeftRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Left;
        GameObject.Find("MagneticRightRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Right;
        GameObject.Find("MagneticTopRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Top;
        GameObject.Find("MagneticBottomRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Bottom;
    }
    [PunRPC]
    void SettingOriginalScale(float LR_Original, float TB_Original)
    {
        GameObject.Find("MagneticLeftRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().originalScale = LR_Original;
        GameObject.Find("MagneticRightRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().originalScale = LR_Original;
        GameObject.Find("MagneticTopRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().originalScale = TB_Original;
        GameObject.Find("MagneticBottomRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().originalScale = TB_Original;
    }
    [PunRPC]
    void SettingPartnerCloud()
    {
        {// 쓰레기코드
            GameObject.Find("MagneticLeftRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().partnerCloud[0] = GameObject.Find("MagneticTopRounge(Clone)");
            GameObject.Find("MagneticLeftRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().partnerCloud[1] = GameObject.Find("MagneticBottomRounge(Clone)");

            GameObject.Find("MagneticRightRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().partnerCloud[0] = GameObject.Find("MagneticTopRounge(Clone)");
            GameObject.Find("MagneticRightRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().partnerCloud[1] = GameObject.Find("MagneticBottomRounge(Clone)");

            GameObject.Find("MagneticTopRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().partnerCloud[0] = GameObject.Find("MagneticLeftRounge(Clone)");
            GameObject.Find("MagneticTopRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().partnerCloud[1] = GameObject.Find("MagneticRightRounge(Clone)");

            GameObject.Find("MagneticBottomRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().partnerCloud[0] = GameObject.Find("MagneticLeftRounge(Clone)");
            GameObject.Find("MagneticBottomRounge(Clone)").GetComponent<MagneticCloudEffectCreator>().partnerCloud[1] = GameObject.Find("MagneticRightRounge(Clone)");

        }
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].GetComponent<TickDamage>().partnerObject.Add(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT]);
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].GetComponent<TickDamage>().partnerObject.Add(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM]);
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].GetComponent<TickDamage>().partnerObject.Add(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP]);

            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].GetComponent<TickDamage>().partnerObject.Add(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT]);
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].GetComponent<TickDamage>().partnerObject.Add(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM]);
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].GetComponent<TickDamage>().partnerObject.Add(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP]);

            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].GetComponent<TickDamage>().partnerObject.Add(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT]);
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].GetComponent<TickDamage>().partnerObject.Add(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT]);
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].GetComponent<TickDamage>().partnerObject.Add(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP]);

            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].GetComponent<TickDamage>().partnerObject.Add(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT]);
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].GetComponent<TickDamage>().partnerObject.Add(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM]);
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].GetComponent<TickDamage>().partnerObject.Add(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT]);

        }
    }

    private void SettingRandomPosition()
    {
        Vector2 minLeftBottom;
        Vector2 maxRightTop;
        // 범위의 최솟값을 구하기 위해 우선은 자기장 시작점 중 left , bottom 값을 넣는다 .
        minLeftBottom.x = leftBottomGround.position.x;
        minLeftBottom.y = leftBottomGround.position.z;

        // 범위의 최대값을 구하기 위해 우선은 자기장 시작점 중 right , top 값을 넣는다 .
        maxRightTop.x = rightTopGround.position.x;
        maxRightTop.y = rightTopGround.position.z;

        // 범위의 최솟값을 자기장 영역의 반을 더해서 보정한다.
        minLeftBottom.x += magneticfieldScale.x / 2;
        minLeftBottom.y += magneticfieldScale.y / 2;

        // 범위의 최대값을 자기장 영역의 반을 빼서 보정한다.
        maxRightTop.x -= magneticfieldScale.x / 2;
        maxRightTop.y -= magneticfieldScale.y / 2;


        magneticfieldpos.x = Random.Range(minLeftBottom.x, maxRightTop.x);
        magneticfieldpos.y = Random.Range(minLeftBottom.y, maxRightTop.y);


        // 최종 줄어들어야 할 크기
        float leftCorrection;
        float rightCorrection;
        float topCorrection;
        float bottomCorrection;


        // 자기장이 줄어들 시간에 맞춰서 비례해서 확장되야 함으로 1초에 확장될 양은 확장량 / 줄어들 초
        leftCorrection = (magneticfieldpos.x - magneticfieldScale.x / 2) - leftTopGround.position.x;
        leftFrameCorrection = leftCorrection / decreaseTime;

        rightCorrection = rightBottomGround.position.x - (magneticfieldpos.x + magneticfieldScale.x / 2);
        rightFrameCorrection = rightCorrection / decreaseTime;

        topCorrection = leftTopGround.position.z - (magneticfieldpos.y + magneticfieldScale.y / 2);
        topFrameCorrection = topCorrection / decreaseTime;

        bottomCorrection = (magneticfieldpos.y - magneticfieldScale.y / 2) - leftBottomGround.position.z;
        bottomFrameCorrection = bottomCorrection / decreaseTime;

        for (float i = damageZoneTimeDivideCount; 0 < i; --i)
        {
            calculatedSafeZones[(int)i - 1].phaseIndex = damageZoneTimeDivideCount;
            calculatedSafeZones[(int)i - 1].left = leftTopGround.position.x + leftCorrection * (i / damageZoneTimeDivideCount);
            calculatedSafeZones[(int)i - 1].right = rightTopGround.position.x - rightCorrection * (i / damageZoneTimeDivideCount);
            calculatedSafeZones[(int)i - 1].top = leftTopGround.position.z - topCorrection * (i / damageZoneTimeDivideCount);
            calculatedSafeZones[(int)i - 1].bottom = rightBottomGround.position.z + bottomCorrection * (i / damageZoneTimeDivideCount);
        }
        calculatedSafeZones[0].phaseIndex = 0;
        calculatedSafeZones[0].left = leftTopGround.position.x;
        calculatedSafeZones[0].right = rightTopGround.position.x;
        calculatedSafeZones[0].top = leftTopGround.position.z;
        calculatedSafeZones[0].bottom = rightBottomGround.position.z;

    }

    void SetEachOther(GameObject newMagneticField, Transform start, Transform end)
    {
        Vector3 pos;
        Vector3 scale;
        Vector3 rot = new Vector3(0, 0, 0);

        if (start.position.x - end.position.x <= float.Epsilon)
        {
            pos.x = start.position.x;
            pos.z = (start.position.z + end.position.z) / 2;
            pos.y = start.position.y + 1;

            scale.y = 1;
            scale.z = 1 * Mathf.Abs(end.position.z - start.position.z);
            scale.x = 0.1f;

            newMagneticField.transform.localPosition = pos;
            newMagneticField.transform.localScale = scale;
        }
        else
        {
            pos.x = (start.position.x + end.position.x) / 2;
            pos.z = start.position.z;
            pos.y = start.position.y + 1;

            scale.y = 1;
            scale.z = 0.1f;
            scale.x = 1 * Mathf.Abs(end.position.x - start.position.x);

            newMagneticField.transform.localPosition = pos;
            newMagneticField.transform.localScale = scale;
        }

    }

    void expansionMangeticField()
    {
        // 확대가 될 양
        float expanstionValue = leftFrameCorrection * Time.deltaTime;
        float correctionValue = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.localScale.x;

        //왼쪽 자기장면 확대
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.localScale = new Vector3(Mathf.Abs(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.localScale.x + expanstionValue)
                                                                                            , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.localScale.y
                                                                                            , Mathf.Abs(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.localScale.z));
        //왼쪽 자기장면 보정
        correctionValue = expanstionValue / 2;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.localPosition = new Vector3(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.position.x + correctionValue
                                                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.position.y
                                                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.position.z);

        //오른쪽 자기장면 확대
        expanstionValue = rightFrameCorrection * Time.deltaTime;
        correctionValue = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.localScale.x;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.localScale = new Vector3(Mathf.Abs(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.localScale.x + expanstionValue)
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.localScale.y
                                                                , Mathf.Abs(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.localScale.z));

        //오른쪽 자기장면 보정
        correctionValue = expanstionValue / 2;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.localPosition = new Vector3(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.position.x - correctionValue
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.position.y
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.position.z);


        //상단 자기장면 확대
        expanstionValue = topFrameCorrection * Time.deltaTime;

        correctionValue = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.localScale.z;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.localScale = new Vector3(Mathf.Abs(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.localScale.x)
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.localScale.y
                                                                , Mathf.Abs(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.localScale.z + expanstionValue));
        //상단 자기장면 보정
        correctionValue = expanstionValue / 2;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.localPosition = new Vector3(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.position.x
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.position.y
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.position.z - correctionValue);



        //하단 자기장면 확대
        expanstionValue = bottomFrameCorrection * Time.deltaTime;

        correctionValue = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale.z;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale = new Vector3(Mathf.Abs(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale.x)
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale.y
                                                                , Mathf.Abs(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale.z + expanstionValue));
        //하단 자기장면 보정
        correctionValue = expanstionValue / 2;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localPosition = new Vector3(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.position.x
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.position.y
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.position.z + correctionValue);



        // Time 관련된 코드들
        decreaseTime -= Time.deltaTime;
        divideTime += Time.deltaTime;
    }


    void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if (false == isReadyField)
        {
            magneticFieldStartTime -= Time.fixedDeltaTime;
            if (0 > magneticFieldStartTime)
            {
                SettingMagneticField();
                isReadyField = true;
            }
            return;
        }

        if(isReadyField)
        {
            if (decreaseOneCircleActiveTime <= divideTime)
            {
                inDivideAccumTime += Time.deltaTime;
                if (divideStayTime <= inDivideAccumTime)
                {
                    ++nowPhaseIndex;
                    inDivideAccumTime = 0.0f;
                    divideTime = 0.0f;
                }
            }
            else
            {
                if (0f <= decreaseTime)
                    expansionMangeticField();
            }
        }

        
    }
}
