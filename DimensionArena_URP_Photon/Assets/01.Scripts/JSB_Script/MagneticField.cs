using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : MonoBehaviour
{
    [SerializeField] Vector2 magneticfieldScale;

    private Vector2 magneticfieldpos;

    [SerializeField] Transform leftTopGround;
    [SerializeField] Transform leftBottomGround;
    [SerializeField] Transform rightTopGround;
    [SerializeField] Transform rightBottomGround;

    [SerializeField] GameObject magneticRect;

    [SerializeField] private float scaleSpeed = 1f;

    private List<GameObject> magneticfieldObj = new List<GameObject>();
    private void SettingRandomPosition()
    {
        // �켱 �ӽ÷� �߾Ӱ����� ������ ���� ����.
        magneticfieldpos.x = leftTopGround.position.x - rightTopGround.position.x / 2;
        magneticfieldpos.y = leftTopGround.position.y - rightTopGround.position.y / 2;

        //Vector2 minLeftBottom;
        //Vector2 maxRightTop;
        //// ������ �ּڰ��� ���ϱ� ���� �켱�� �ڱ��� ������ �� left , bottom ���� �ִ´� .
        //minLeftBottom.x = leftBottomGround.position.x;
        //minLeftBottom.y = leftBottomGround.position.z;
        //
        //// ������ �ִ밪�� ���ϱ� ���� �켱�� �ڱ��� ������ �� right , top ���� �ִ´� .
        //maxRightTop.x = rightTopGround.position.x;
        //maxRightTop.y = rightTopGround.position.z;
        //
        //// ������ �ּڰ��� �ڱ��� ������ ���� ���ؼ� �����Ѵ�.
        //minLeftBottom.x += magneticfieldScale.x / 2;
        //minLeftBottom.y += magneticfieldScale.y / 2;
        //
        //// ������ �ִ밪�� �ڱ��� ������ ���� ���� �����Ѵ�.
        //maxRightTop.x -= magneticfieldScale.x / 2;
        //maxRightTop.y -= magneticfieldScale.y / 2;

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
        float expanstionValue = scaleSpeed * Time.deltaTime;
        float correctionValue = magneticfieldObj[0].transform.localScale.x;

        //���� �ڱ���� Ȯ��
        magneticfieldObj[0].transform.localScale = new Vector3(magneticfieldObj[0].transform.localScale.x + expanstionValue
                                                                , magneticfieldObj[0].transform.localScale.y
                                                                , magneticfieldObj[0].transform.localScale.z);
        //���� �ڱ���� ����
        correctionValue -= magneticfieldObj[0].transform.localScale.x;
        Mathf.Abs(correctionValue);
        correctionValue /= 2;
        magneticfieldObj[0].transform.localPosition = new Vector3(magneticfieldObj[0].transform.position.x - correctionValue
                                                                , magneticfieldObj[0].transform.position.y
                                                                , magneticfieldObj[0].transform.position.z);

        //������ �ڱ���� Ȯ��
        correctionValue = magneticfieldObj[1].transform.localScale.x;
        magneticfieldObj[1].transform.localScale = new Vector3(magneticfieldObj[1].transform.localScale.x + expanstionValue
                                                                , magneticfieldObj[1].transform.localScale.y
                                                                , magneticfieldObj[1].transform.localScale.z);

        //������ �ڱ���� ����
        correctionValue -= magneticfieldObj[1].transform.localScale.x;
        Mathf.Abs(correctionValue);
        correctionValue /= 2;
        magneticfieldObj[1].transform.localPosition = new Vector3(magneticfieldObj[1].transform.position.x + correctionValue
                                                                , magneticfieldObj[1].transform.position.y
                                                                , magneticfieldObj[1].transform.position.z);


        //��� �ڱ���� Ȯ��
        correctionValue = magneticfieldObj[2].transform.localScale.z;
        magneticfieldObj[2].transform.localScale = new Vector3(magneticfieldObj[2].transform.localScale.x
                                                                , magneticfieldObj[2].transform.localScale.y
                                                                , magneticfieldObj[2].transform.localScale.z + expanstionValue);
        //��� �ڱ���� ����
        correctionValue -= magneticfieldObj[2].transform.localScale.z;
        Mathf.Abs(correctionValue);
        correctionValue /= 2;
        magneticfieldObj[2].transform.localPosition = new Vector3(magneticfieldObj[2].transform.position.x 
                                                                , magneticfieldObj[2].transform.position.y
                                                                , magneticfieldObj[2].transform.position.z + correctionValue);



        //�ϴ� �ڱ���� Ȯ��
        correctionValue = magneticfieldObj[3].transform.localScale.z;
        magneticfieldObj[3].transform.localScale = new Vector3(magneticfieldObj[3].transform.localScale.x
                                                                , magneticfieldObj[3].transform.localScale.y
                                                                , magneticfieldObj[3].transform.localScale.z + expanstionValue);
        //�ϴ� �ڱ���� ����
        correctionValue -= magneticfieldObj[3].transform.localScale.z;
        Mathf.Abs(correctionValue);
        correctionValue /= 2;
        magneticfieldObj[3].transform.localPosition = new Vector3(magneticfieldObj[3].transform.position.x
                                                                , magneticfieldObj[3].transform.position.y
                                                                , magneticfieldObj[3].transform.position.z - correctionValue);


    }

    void Start()
    {
        // ���� ��
        SetEachOther(leftTopGround, leftBottomGround);
        magneticfieldObj.Add(Instantiate(magneticRect));
        // ������ ��
        SetEachOther(rightTopGround, rightBottomGround);
        magneticfieldObj.Add(Instantiate(magneticRect));

        // ��� ��
        SetEachOther(rightTopGround, leftTopGround);
        magneticfieldObj.Add(Instantiate(magneticRect));
        // �ϴ� ��
        SetEachOther(rightBottomGround, leftBottomGround);
        magneticfieldObj.Add(Instantiate(magneticRect));
    }

    void FixedUpdate()
    {

        expansionMangeticField();
    }
}
