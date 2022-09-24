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
        // ������ �ּڰ��� ���ϱ� ���� �켱�� �ڱ��� ������ �� left , bottom ���� �ִ´� .
        minLeftBottom.x = leftBottomGround.position.x;
        minLeftBottom.y = leftBottomGround.position.z;

        // ������ �ִ밪�� ���ϱ� ���� �켱�� �ڱ��� ������ �� right , top ���� �ִ´� .
        maxRightTop.x = rightTopGround.position.x;
        maxRightTop.y = rightTopGround.position.z;

        // ������ �ּڰ��� �ڱ��� ������ ���� ���ؼ� �����Ѵ�.
        minLeftBottom.x += magneticfieldScale.x / 2;
        minLeftBottom.y += magneticfieldScale.y / 2;

        // ������ �ִ밪�� �ڱ��� ������ ���� ���� �����Ѵ�.
        maxRightTop.x -= magneticfieldScale.x / 2;
        maxRightTop.y -= magneticfieldScale.y / 2;


        magneticfieldpos.x = Random.Range(minLeftBottom.x, maxRightTop.x);
        magneticfieldpos.y = Random.Range(minLeftBottom.y, maxRightTop.y);


        // �ڱ����� �پ�� �ð��� ���缭 ����ؼ� Ȯ��Ǿ� ������ 1�ʿ� Ȯ��� ���� Ȯ�差 / �پ�� ��
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
        // Ȯ�밡 �� ��
        float expanstionValue = leftCorrection * Time.deltaTime;
        float correctionValue = magneticfieldObj[0].transform.localScale.x;

        //���� �ڱ���� Ȯ��
        magneticfieldObj[0].transform.localScale = new Vector3(magneticfieldObj[0].transform.localScale.x + expanstionValue
                                                                , magneticfieldObj[0].transform.localScale.y
                                                                , magneticfieldObj[0].transform.localScale.z);
        //���� �ڱ���� ����
        correctionValue = expanstionValue / 2;
        magneticfieldObj[0].transform.localPosition = new Vector3(magneticfieldObj[0].transform.position.x + correctionValue
                                                                , magneticfieldObj[0].transform.position.y
                                                                , magneticfieldObj[0].transform.position.z);

        //������ �ڱ���� Ȯ��
        expanstionValue = rightCorrection * Time.deltaTime;
        correctionValue = magneticfieldObj[1].transform.localScale.x;
        magneticfieldObj[1].transform.localScale = new Vector3(magneticfieldObj[1].transform.localScale.x + expanstionValue
                                                                , magneticfieldObj[1].transform.localScale.y
                                                                , magneticfieldObj[1].transform.localScale.z);

        //������ �ڱ���� ����
        correctionValue = expanstionValue / 2;
        magneticfieldObj[1].transform.localPosition = new Vector3(magneticfieldObj[1].transform.position.x - correctionValue
                                                                , magneticfieldObj[1].transform.position.y
                                                                , magneticfieldObj[1].transform.position.z);


        //��� �ڱ���� Ȯ��
        expanstionValue = topCorrection * Time.deltaTime;

        correctionValue = magneticfieldObj[2].transform.localScale.z;
        magneticfieldObj[2].transform.localScale = new Vector3(magneticfieldObj[2].transform.localScale.x
                                                                , magneticfieldObj[2].transform.localScale.y
                                                                , magneticfieldObj[2].transform.localScale.z + expanstionValue);
        //��� �ڱ���� ����
        correctionValue = expanstionValue / 2;
        magneticfieldObj[2].transform.localPosition = new Vector3(magneticfieldObj[2].transform.position.x 
                                                                , magneticfieldObj[2].transform.position.y
                                                                , magneticfieldObj[2].transform.position.z - correctionValue);



        //�ϴ� �ڱ���� Ȯ��
        expanstionValue = bottomCorrection   * Time.deltaTime;

        correctionValue = magneticfieldObj[3].transform.localScale.z;
        magneticfieldObj[3].transform.localScale = new Vector3(magneticfieldObj[3].transform.localScale.x
                                                                , magneticfieldObj[3].transform.localScale.y
                                                                , magneticfieldObj[3].transform.localScale.z + expanstionValue);
        //�ϴ� �ڱ���� ����
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
        // ���� ��
        SetEachOther(leftTopGround, leftBottomGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Left;
        magneticfieldObj.Add(cloud);
        
        // ������ ��
        SetEachOther(rightTopGround, rightBottomGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Right;
        magneticfieldObj.Add(cloud);
        
        // ��� ��
        SetEachOther(rightTopGround, leftTopGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Top;
        magneticfieldObj.Add(cloud);
        
        // �ϴ� ��
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
