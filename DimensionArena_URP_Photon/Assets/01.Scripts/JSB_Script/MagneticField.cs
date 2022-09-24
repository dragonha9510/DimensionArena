using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SafeZone 
{ 
    public float left; 
    public float right; 
    public float top; 
    public float bottom; 
    public float height { get { return top - bottom; } }
    public float width { get { return right - left; } }

}

public class MagneticField : MonoBehaviour
{
    [SerializeField] float decreaseTime = 10.0f;

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
        float correctionValue = magneticfieldObj[0].transform.localScale.x;

        //왼쪽 자기장면 확대
        magneticfieldObj[0].transform.localScale = new Vector3(magneticfieldObj[0].transform.localScale.x + expanstionValue
                                                                , magneticfieldObj[0].transform.localScale.y
                                                                , magneticfieldObj[0].transform.localScale.z);
        //왼쪽 자기장면 보정
        correctionValue = expanstionValue / 2;
        magneticfieldObj[0].transform.localPosition = new Vector3(magneticfieldObj[0].transform.position.x + correctionValue
                                                                , magneticfieldObj[0].transform.position.y
                                                                , magneticfieldObj[0].transform.position.z);

        //오른쪽 자기장면 확대
        expanstionValue = rightCorrection * Time.deltaTime;
        correctionValue = magneticfieldObj[1].transform.localScale.x;
        magneticfieldObj[1].transform.localScale = new Vector3(magneticfieldObj[1].transform.localScale.x + expanstionValue
                                                                , magneticfieldObj[1].transform.localScale.y
                                                                , magneticfieldObj[1].transform.localScale.z);

        //오른쪽 자기장면 보정
        correctionValue = expanstionValue / 2;
        magneticfieldObj[1].transform.localPosition = new Vector3(magneticfieldObj[1].transform.position.x - correctionValue
                                                                , magneticfieldObj[1].transform.position.y
                                                                , magneticfieldObj[1].transform.position.z);


        //상단 자기장면 확대
        expanstionValue = topCorrection * Time.deltaTime;

        correctionValue = magneticfieldObj[2].transform.localScale.z;
        magneticfieldObj[2].transform.localScale = new Vector3(magneticfieldObj[2].transform.localScale.x
                                                                , magneticfieldObj[2].transform.localScale.y
                                                                , magneticfieldObj[2].transform.localScale.z + expanstionValue);
        //상단 자기장면 보정
        correctionValue = expanstionValue / 2;
        magneticfieldObj[2].transform.localPosition = new Vector3(magneticfieldObj[2].transform.position.x 
                                                                , magneticfieldObj[2].transform.position.y
                                                                , magneticfieldObj[2].transform.position.z - correctionValue);



        //하단 자기장면 확대
        expanstionValue = bottomCorrection   * Time.deltaTime;

        correctionValue = magneticfieldObj[3].transform.localScale.z;
        magneticfieldObj[3].transform.localScale = new Vector3(magneticfieldObj[3].transform.localScale.x
                                                                , magneticfieldObj[3].transform.localScale.y
                                                                , magneticfieldObj[3].transform.localScale.z + expanstionValue);
        //하단 자기장면 보정
        correctionValue = expanstionValue / 2;
        magneticfieldObj[3].transform.localPosition = new Vector3(magneticfieldObj[3].transform.position.x
                                                                , magneticfieldObj[3].transform.position.y
                                                                , magneticfieldObj[3].transform.position.z + correctionValue);

        decreaseTime -= Time.deltaTime;
        Bounds bounds;
        bounds = magneticfieldObj[0].GetComponent<Collider>().bounds;
        safeZone.left = magneticfieldObj[0].transform.position.x + bounds.size.x / 2;
        
        bounds = magneticfieldObj[1].GetComponent<Collider>().bounds;
        safeZone.right = magneticfieldObj[1].transform.position.x - bounds.size.x / 2;
        
        bounds = magneticfieldObj[2].GetComponent<Collider>().bounds;
        safeZone.top = magneticfieldObj[2].transform.position.z - bounds.size.z / 2;

        bounds = magneticfieldObj[3].GetComponent<Collider>().bounds;
        safeZone.bottom = magneticfieldObj[3].transform.position.z + bounds.size.z / 2;
    }

    void Start()
    {
        GameObject cloud;
        // 왼쪽 면
        SetEachOther(leftTopGround, leftBottomGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Left;
        magneticfieldObj.Add(cloud);
        
        // 오른쪽 면
        SetEachOther(rightTopGround, rightBottomGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Right;
        magneticfieldObj.Add(cloud);
        
        // 상단 면
        SetEachOther(rightTopGround, leftTopGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Top;
        magneticfieldObj.Add(cloud);
        
        // 하단 면
        SetEachOther(rightBottomGround, leftBottomGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Bottom;
        magneticfieldObj.Add(cloud);

        SettingRandomPosition();
    }

    void FixedUpdate()
    {
        if (0f <= decreaseTime)
            expansionMangeticField();
    }
}
