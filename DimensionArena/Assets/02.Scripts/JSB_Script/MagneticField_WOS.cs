using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MagneticField_WOS : MonoBehaviour
{
    private enum CLOUDTYPE
    {
        CLOUDTYPE_LEFT,
        CLOUDTYPE_RIGHT,
        CLOUDTYPE_TOP,
        CLOUDTYPE_BOTOTM
    }
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
        float correctionValue = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.localScale.x;

        //���� �ڱ���� Ȯ��
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.localScale = new Vector3(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.localScale.x + expanstionValue
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.localScale.y
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.localScale.z);
        //���� �ڱ���� ����
        correctionValue = expanstionValue / 2;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.localPosition = new Vector3(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.position.x + correctionValue
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.position.y
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.position.z);

        //������ �ڱ���� Ȯ��
        expanstionValue = rightCorrection * Time.deltaTime;
        correctionValue = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.localScale.x;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.localScale = new Vector3(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.localScale.x + expanstionValue
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.localScale.y
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.localScale.z);

        //������ �ڱ���� ����
        correctionValue = expanstionValue / 2;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.localPosition = new Vector3(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.position.x - correctionValue
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.position.y
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.position.z);


        //��� �ڱ���� Ȯ��
        expanstionValue = topCorrection * Time.deltaTime;

        correctionValue = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.localScale.z;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.localScale = new Vector3(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.localScale.x
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.localScale.y
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.localScale.z + expanstionValue);
        //��� �ڱ���� ����
        correctionValue = expanstionValue / 2;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.localPosition = new Vector3(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.position.x
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.position.y
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.position.z - correctionValue);



        //�ϴ� �ڱ���� Ȯ��
        expanstionValue = bottomCorrection * Time.deltaTime;

        correctionValue = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale.z;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale = new Vector3(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale.x
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale.y
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localScale.z + expanstionValue);
        //�ϴ� �ڱ���� ����
        correctionValue = expanstionValue / 2;
        magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.localPosition = new Vector3(magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.position.x
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.position.y
                                                                , magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.position.z + correctionValue);

        decreaseTime -= Time.deltaTime;
        Bounds bounds;
        bounds = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].GetComponent<Collider>().bounds;
        safeZone.left = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_LEFT].transform.position.x + bounds.size.x / 2;

        bounds = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].GetComponent<Collider>().bounds;
        safeZone.right = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_RIGHT].transform.position.x - bounds.size.x / 2;

        bounds = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].GetComponent<Collider>().bounds;
        safeZone.top = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_TOP].transform.position.z - bounds.size.z / 2;

        bounds = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].GetComponent<Collider>().bounds;
        safeZone.bottom = magneticfieldObj[(int)CLOUDTYPE.CLOUDTYPE_BOTOTM].transform.position.z + bounds.size.z / 2;
    }

    void Start()
    {
        GameObject cloud;
        float originalScale;
        // ���� ��
        SetEachOther(leftTopGround, leftBottomGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Left;
        originalScale = ((rightTopGround.position.x - leftTopGround.position.x) / 2) * (leftTopGround.position.z - leftBottomGround.position.z);
        cloud.GetComponent<MagneticCloudEffectCreator>().originalScale = originalScale;
        magneticfieldObj.Add(cloud);

        // ������ ��
        SetEachOther(rightTopGround, rightBottomGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Right;
        cloud.GetComponent<MagneticCloudEffectCreator>().originalScale = originalScale;
        magneticfieldObj.Add(cloud);

        // ��� ��
        SetEachOther(rightTopGround, leftTopGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Top;
        originalScale = ((rightTopGround.position.z - rightBottomGround.position.z) / 2) * (rightTopGround.position.x - leftTopGround.position.x);
        cloud.GetComponent<MagneticCloudEffectCreator>().originalScale = originalScale;


        magneticfieldObj.Add(cloud);

        // �ϴ� ��
        SetEachOther(rightBottomGround, leftBottomGround);
        cloud = Instantiate(magneticRect);
        cloud.GetComponent<MagneticCloudEffectCreator>().cloudType = MagneticCloudPos.MagneticCloudPos_Bottom;
        cloud.GetComponent<MagneticCloudEffectCreator>().originalScale = originalScale;

        magneticfieldObj.Add(cloud);



        {// �������ڵ�
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

    void FixedUpdate()
    {
        if (0f <= decreaseTime)
            expansionMangeticField();
    }
}