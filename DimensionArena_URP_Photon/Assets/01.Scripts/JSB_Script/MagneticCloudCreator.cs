using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum MagneticCloudPos
{
    MagneticCloudPos_Right,
    MagneticCloudPos_Left,
    MagneticCloudPos_Top,
    MagneticCloudPos_Bottom
}
public class MagneticCloudCreator : MonoBehaviour
{
    // 구름 생성 열 간격 입니다 . 
    [SerializeField]
    private float columnSpacing;
    // 구름 생성 행 간격 입니다 . 
    [SerializeField]
    private float rowSpacing;

    [SerializeField]
    private GameObject magneticCloud;

    private Bounds colliderBounds;

    private Vector3 startVec;

    // 구름 생성 타입입니다 , 상하좌우 입니다.
    private MagneticCloudPos cloudType;


    private void CreateCloud(MagneticCloudPos type)
    {
        switch(type)
        {
            case MagneticCloudPos.MagneticCloudPos_Left:
            case MagneticCloudPos.MagneticCloudPos_Right:
                startVec.x = transform.position.x;
                startVec.y = transform.position.y;
                startVec.z = transform.position.z - colliderBounds.size.z / 2;
                
                for(int i = 0 ; startVec.z + (i* columnSpacing) < transform.position.z + colliderBounds.size.z / 2; ++i)
                {
                    Vector3 pos;
                    Transform trans = this.transform;
                    pos = startVec;
                    pos.z += (i * columnSpacing);
                    trans.position = pos;
                    Instantiate(magneticCloud,trans);
                }
                break;
            case MagneticCloudPos.MagneticCloudPos_Top:
                break;
            case MagneticCloudPos.MagneticCloudPos_Bottom:
                break;
        }
    }

    void Start()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        colliderBounds = collider.GetComponent<Bounds>();
        CreateCloud(MagneticCloudPos.MagneticCloudPos_Left);


    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
