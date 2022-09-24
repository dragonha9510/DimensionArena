using System.Collections;
using System.Collections.Generic;

using UnityEngine;
public enum MagneticCloudPos
{
    MagneticCloudPos_Right,
    MagneticCloudPos_Left,
    MagneticCloudPos_Top,
    MagneticCloudPos_Bottom
}
public class MagneticCloudEffectCreator : MonoBehaviour
{

    [SerializeField]
    private int createCloudCount = 3;
    // 구름 생성 간격 입니다 . ( 가에 구름만 )
    [SerializeField]
    private float cloudSpacing;


    [SerializeField]
    private float randomSpacingTime;
    [SerializeField]
    private float spacingTime;
    
    private WaitForSeconds cloudRandomSpacingTime = new WaitForSeconds(0);
    private WaitForSeconds cloudSpacingTime = new WaitForSeconds(0);
    
    
    [SerializeField]
    private GameObject magneticCloud;


    private Bounds colliderBounds;

    private Vector2 outLineCloudRange;

    private DelayMachine delayMachine = new DelayMachine();

    // 구름 생성 타입입니다 , 상하좌우 입니다.
    public MagneticCloudPos cloudType;

    // 랜덤생성을 위한 x,z 값을 저장하기 위한 벡터
    Vector2 randomXRange;
    Vector2 randomZRange;


    void Start()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        colliderBounds = collider.bounds;

        cloudRandomSpacingTime = new WaitForSeconds(randomSpacingTime);
        cloudSpacingTime = new WaitForSeconds(spacingTime);

        SettingCloudLine();
        StartCoroutine("CreateEdgeCloud");
        StartCoroutine("CreateRandomCloud");
    }
    private void SettingCloudLine()
    {

        switch (cloudType)
        {
            // 왼쪽 오른쪽은 z 축 기준으로 위에서 밑으로 구름들을 생성해야 한다.
            case MagneticCloudPos.MagneticCloudPos_Left:
            case MagneticCloudPos.MagneticCloudPos_Right:
                outLineCloudRange.x = this.transform.position.z + colliderBounds.size.z / 2;
                outLineCloudRange.y = this.transform.position.z - colliderBounds.size.z / 2;
                break;
            // 위쪽 아래쪽은 x 축 기준으로 위에서 왼쪽에서 오른쪽으로 구름들을 생성해야 한다.
            case MagneticCloudPos.MagneticCloudPos_Top:
            case MagneticCloudPos.MagneticCloudPos_Bottom:
                outLineCloudRange.x = this.transform.position.x - colliderBounds.size.x / 2;
                outLineCloudRange.y = this.transform.position.x + colliderBounds.size.x / 2;
                break;
        }
    }
    IEnumerator CreateEdgeCloud()
    {
        while(true)
        {
            switch (cloudType)
                {
                    // 왼쪽은 z 축 기준으로 위에서 밑으로 구름들을 생성해야 한다.
                    // 그렇다면 고정된 값은 x 축이 고정되어 있을 것 이다.
                    case MagneticCloudPos.MagneticCloudPos_Left:
                    case MagneticCloudPos.MagneticCloudPos_Right:
                        for (float z = outLineCloudRange.x; z > outLineCloudRange.y; z -= cloudSpacing)
                        {
                            GameObject cloud = Instantiate(magneticCloud);
                            cloud.transform.position = new Vector3(this.transform.position.x + (this.transform.localScale.x / 2) * (cloudType == MagneticCloudPos.MagneticCloudPos_Right ? -1 : 1 )
                                                                    , this.transform.position.y
                                                                    , z);
                        }
                        break;
                    case MagneticCloudPos.MagneticCloudPos_Top:
                    case MagneticCloudPos.MagneticCloudPos_Bottom:
                        for (float x = outLineCloudRange.x; x < outLineCloudRange.y; x += cloudSpacing)
                        {
                            GameObject cloud = Instantiate(magneticCloud);
                            cloud.transform.position = new Vector3(x
                                                                   , this.transform.position.y
                                                                   , this.transform.position.z + (this.transform.localScale.z / 2) * (cloudType == MagneticCloudPos.MagneticCloudPos_Top ? -1 : 1));
                        }
                        break;
                }
                yield return cloudSpacingTime;
        }
    }
    IEnumerator CreateRandomCloud()
    {
        while (true)
        {
            randomXRange.x = this.transform.position.x - this.transform.localScale.x / 2;
            randomXRange.y = this.transform.position.x + this.transform.localScale.x / 2;
            randomZRange.x = this.transform.position.z - this.transform.localScale.z / 2;
            randomZRange.y = this.transform.position.z + this.transform.localScale.z / 2;
            for(int i = 0; i < createCloudCount; ++i)
            {
                Vector3 randomPosition = new Vector3(Random.Range(randomXRange.x, randomXRange.y), this.transform.position.y, Random.Range(randomZRange.x, randomZRange.y));
                GameObject cloud = Instantiate(magneticCloud);
                cloud.transform.position = randomPosition;
            }
            yield return cloudRandomSpacingTime;
        }
    }


    // Update is called once per frame
    void Update()
    {
    }


}
