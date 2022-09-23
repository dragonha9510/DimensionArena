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
        // 우선 임시로 중앙값으로 설정을 시켜 놓자.
        magneticfieldpos.x = leftTopGround.position.x - rightTopGround.position.x / 2;
        magneticfieldpos.y = leftTopGround.position.y - rightTopGround.position.y / 2;

        //Vector2 minLeftBottom;
        //Vector2 maxRightTop;
        //// 범위의 최솟값을 구하기 위해 우선은 자기장 시작점 중 left , bottom 값을 넣는다 .
        //minLeftBottom.x = leftBottomGround.position.x;
        //minLeftBottom.y = leftBottomGround.position.z;
        //
        //// 범위의 최대값을 구하기 위해 우선은 자기장 시작점 중 right , top 값을 넣는다 .
        //maxRightTop.x = rightTopGround.position.x;
        //maxRightTop.y = rightTopGround.position.z;
        //
        //// 범위의 최솟값을 자기장 영역의 반을 더해서 보정한다.
        //minLeftBottom.x += magneticfieldScale.x / 2;
        //minLeftBottom.y += magneticfieldScale.y / 2;
        //
        //// 범위의 최대값을 자기장 영역의 반을 빼서 보정한다.
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
        // 확대가 될 양
        float expanstionValue = scaleSpeed * Time.deltaTime;
        float correctionValue = magneticfieldObj[0].transform.localScale.x;

        //왼쪽 자기장면 확대
        magneticfieldObj[0].transform.localScale = new Vector3(magneticfieldObj[0].transform.localScale.x + expanstionValue
                                                                , magneticfieldObj[0].transform.localScale.y
                                                                , magneticfieldObj[0].transform.localScale.z);
        //왼쪽 자기장면 보정
        correctionValue -= magneticfieldObj[0].transform.localScale.x;
        Mathf.Abs(correctionValue);
        correctionValue /= 2;
        magneticfieldObj[0].transform.localPosition = new Vector3(magneticfieldObj[0].transform.position.x - correctionValue
                                                                , magneticfieldObj[0].transform.position.y
                                                                , magneticfieldObj[0].transform.position.z);

        //오른쪽 자기장면 확대
        correctionValue = magneticfieldObj[1].transform.localScale.x;
        magneticfieldObj[1].transform.localScale = new Vector3(magneticfieldObj[1].transform.localScale.x + expanstionValue
                                                                , magneticfieldObj[1].transform.localScale.y
                                                                , magneticfieldObj[1].transform.localScale.z);

        //오른쪽 자기장면 보정
        correctionValue -= magneticfieldObj[1].transform.localScale.x;
        Mathf.Abs(correctionValue);
        correctionValue /= 2;
        magneticfieldObj[1].transform.localPosition = new Vector3(magneticfieldObj[1].transform.position.x + correctionValue
                                                                , magneticfieldObj[1].transform.position.y
                                                                , magneticfieldObj[1].transform.position.z);


        //상단 자기장면 확대
        correctionValue = magneticfieldObj[2].transform.localScale.z;
        magneticfieldObj[2].transform.localScale = new Vector3(magneticfieldObj[2].transform.localScale.x
                                                                , magneticfieldObj[2].transform.localScale.y
                                                                , magneticfieldObj[2].transform.localScale.z + expanstionValue);
        //상단 자기장면 보정
        correctionValue -= magneticfieldObj[2].transform.localScale.z;
        Mathf.Abs(correctionValue);
        correctionValue /= 2;
        magneticfieldObj[2].transform.localPosition = new Vector3(magneticfieldObj[2].transform.position.x 
                                                                , magneticfieldObj[2].transform.position.y
                                                                , magneticfieldObj[2].transform.position.z + correctionValue);



        //하단 자기장면 확대
        correctionValue = magneticfieldObj[3].transform.localScale.z;
        magneticfieldObj[3].transform.localScale = new Vector3(magneticfieldObj[3].transform.localScale.x
                                                                , magneticfieldObj[3].transform.localScale.y
                                                                , magneticfieldObj[3].transform.localScale.z + expanstionValue);
        //하단 자기장면 보정
        correctionValue -= magneticfieldObj[3].transform.localScale.z;
        Mathf.Abs(correctionValue);
        correctionValue /= 2;
        magneticfieldObj[3].transform.localPosition = new Vector3(magneticfieldObj[3].transform.position.x
                                                                , magneticfieldObj[3].transform.position.y
                                                                , magneticfieldObj[3].transform.position.z - correctionValue);


    }

    void Start()
    {
        // 왼쪽 면
        SetEachOther(leftTopGround, leftBottomGround);
        magneticfieldObj.Add(Instantiate(magneticRect));
        // 오른쪽 면
        SetEachOther(rightTopGround, rightBottomGround);
        magneticfieldObj.Add(Instantiate(magneticRect));

        // 상단 면
        SetEachOther(rightTopGround, leftTopGround);
        magneticfieldObj.Add(Instantiate(magneticRect));
        // 하단 면
        SetEachOther(rightBottomGround, leftBottomGround);
        magneticfieldObj.Add(Instantiate(magneticRect));
    }

    void FixedUpdate()
    {

        expansionMangeticField();
    }
}
