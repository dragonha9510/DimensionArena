using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public struct SafeZone 
{ 
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

    private float decreaseOneCircleActiveTime;

    [SerializeField] Vector2 magneticfieldScale;
    
    [SerializeField] Transform leftTopGround;
    [SerializeField] Transform leftBottomGround;
    [SerializeField] Transform rightTopGround;
    [SerializeField] Transform rightBottomGround;

    [SerializeField] GameObject magneticRect;


    private SafeZone safeZone = new SafeZone();
    public SafeZone GetSafe => safeZone;

    private float leftCorrection;
    private float rightCorrection;
    private float topCorrection;
    private float bottomCorrection;

    private List<GameObject> magneticfieldObj = new List<GameObject>();
    private Vector2 magneticfieldpos;

    void Start()
    {
        // 확장 주기 시간 설정
        decreaseOneCircleActiveTime = decreaseTime / (float)damageZoneTimeDivideCount;

        GameObject cloud;
        float originalScale;
        // 왼쪽 면
        SetEachOther(leftTopGround, leftBottomGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Left;
        originalScale = ((rightTopGround.position.x - leftTopGround.position.x) / 2) * (leftTopGround.position.z - leftBottomGround.position.z);
        cloud.GetComponent<MagneticCloudEffectCreator>().originalScale = originalScale;
        magneticfieldObj.Add(cloud);

        // 오른쪽 면
        SetEachOther(rightTopGround, rightBottomGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Right;
        cloud.GetComponent<MagneticCloudEffectCreator>().originalScale = originalScale;
        magneticfieldObj.Add(cloud);

        // 상단 면
        SetEachOther(rightTopGround, leftTopGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Top;
        originalScale = ((rightTopGround.position.z - rightBottomGround.position.z) / 2) * (rightTopGround.position.x - leftTopGround.position.x);
        cloud.GetComponent<MagneticCloudEffectCreator>().originalScale = originalScale;


        magneticfieldObj.Add(cloud);

        // 하단 면
        SetEachOther(rightBottomGround, leftBottomGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Bottom;
        cloud.GetComponent<MagneticCloudEffectCreator>().originalScale = originalScale;

        magneticfieldObj.Add(cloud);



        {// 쓰레기코드
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].GetComponent<MagneticCloudEffectCreator>().partnerCloud[0] = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP];
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].GetComponent<MagneticCloudEffectCreator>().partnerCloud[1] = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM];

            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].GetComponent<MagneticCloudEffectCreator>().partnerCloud[0] = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP];
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].GetComponent<MagneticCloudEffectCreator>().partnerCloud[1] = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM];

            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].GetComponent<MagneticCloudEffectCreator>().partnerCloud[0] = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT];
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].GetComponent<MagneticCloudEffectCreator>().partnerCloud[1] = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT];

            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].GetComponent<MagneticCloudEffectCreator>().partnerCloud[0] = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT];
            magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].GetComponent<MagneticCloudEffectCreator>().partnerCloud[1] = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT];

        }


        SettingRandomPosition();
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


        // 자기장이 줄어들 시간에 맞춰서 비례해서 확장되야 함으로 1초에 확장될 양은 확장량 / 줄어들 초
        leftCorrection = (magneticfieldpos.x - magneticfieldScale.x / 2) - leftTopGround.position.x;
        leftCorrection = leftCorrection / decreaseTime;
        
        rightCorrection = rightBottomGround.position.x - (magneticfieldpos.x + magneticfieldScale.x / 2);
        rightCorrection = rightCorrection / decreaseTime;
        
        topCorrection = leftTopGround.position.z - (magneticfieldpos.y + magneticfieldScale.y / 2);
        topCorrection = topCorrection / decreaseTime;
        
        bottomCorrection = (magneticfieldpos.y - magneticfieldScale.y / 2) - leftBottomGround.position.z;
        bottomCorrection = bottomCorrection / decreaseTime;
    }

    void SetEachOther(Transform start, Transform end)
    {
        Vector3 pos;
        Vector3 scale;
        Vector3 rot = new Vector3(0, 0, 0);

        if (start.position.x == end.position.x)
        {
            pos.x = start.position.x;
            pos.z = (start.position.z + end.position.z) / 2;
            pos.y = start.position.y + 1;

            scale.y = 1;
            scale.z = 1 * (end.position.z - start.position.z);
            scale.x = 0.1f;

            magneticRect.transform.localPosition = pos;
            magneticRect.transform.localScale = scale;
        }
        else
        {
            pos.x = (start.position.x + end.position.x) / 2;
            pos.z = start.position.z;
            pos.y = start.position.y + 1;

            scale.y = 1;
            scale.z = 0.1f;
            scale.x = 1 * (end.position.x - start.position.x);

            magneticRect.transform.localPosition = pos;
            magneticRect.transform.localScale = scale;
        }
        
    }

    void expansionMangeticField()
    {
        // 확대가 될 양
        float expanstionValue = leftCorrection * Time.deltaTime;
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
        expanstionValue = rightCorrection * Time.deltaTime;
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
        expanstionValue = topCorrection * Time.deltaTime;

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
        expanstionValue = bottomCorrection   * Time.deltaTime;

        correctionValue = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale.z;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale = new Vector3(Mathf.Abs(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale.x)
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale.y
                                                                , Mathf.Abs(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale.z + expanstionValue));
        //하단 자기장면 보정
        correctionValue = expanstionValue / 2;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localPosition = new Vector3(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.position.x
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.position.y
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.position.z + correctionValue);


        safeZone.left = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.position.x + this.transform.localScale.x / 2;
        
        safeZone.right = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.position.x - this.transform.localScale.x / 2;
        
        safeZone.top = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.position.z - this.transform.localScale.z / 2;

        safeZone.bottom = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.position.z + this.transform.localScale.z / 2;




        // Time 관련된 코드들
        decreaseTime -= Time.deltaTime;
        divideTime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        if(decreaseOneCircleActiveTime <= divideTime)
        {
            inDivideAccumTime += Time.deltaTime;
            if (divideStayTime <= inDivideAccumTime)
            {
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
