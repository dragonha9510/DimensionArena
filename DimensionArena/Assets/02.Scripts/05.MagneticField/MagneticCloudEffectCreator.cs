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
    // ���� ���� ���� �Դϴ� . ( ���� ������ )
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

    // ���� ��ģ���� Ȯ�밡 �Ǿ���� ũ�� ���� ����!
    public float originalScale;

    public GameObject[] partnerCloud = new GameObject[2];



    private Vector2 outLineCloudRange;

    private DelayMachine delayMachine = new DelayMachine();

    // ���� ���� Ÿ���Դϴ� , �����¿� �Դϴ�.
    public MagneticCloudPos cloudType;

    // ���������� ���� x,z ���� �����ϱ� ���� ����
    Vector2 randomXRange;
    Vector2 randomZRange;


    Vector3 prevScale;



    void Start()
    {
        prevScale = this.transform.localScale;

        cloudRandomSpacingTime = new WaitForSeconds(randomSpacingTime);
        cloudSpacingTime = new WaitForSeconds(spacingTime);

        StartCoroutine("CreateEdgeCloud");
        StartCoroutine("CreateRandomCloud");
    }
    IEnumerator CreateEdgeCloud()
    {
        while (true)
        {
                if (null != partnerCloud)
                {

                    switch (cloudType)
                    {
                        // ���� �������� z �� �������� ������ ������ �������� �����ؾ� �Ѵ�.
                        case MagneticCloudPos.MagneticCloudPos_Left:
                        case MagneticCloudPos.MagneticCloudPos_Right:
                            outLineCloudRange.x = this.transform.position.z + Mathf.Abs(this.transform.localScale.z) / 2;
                            outLineCloudRange.y = this.transform.position.z - Mathf.Abs(this.transform.localScale.z) / 2;
                            {// �ߺ��ڵ�
                             // ���� ��� ���̱�
                                outLineCloudRange.x -= partnerCloud[0].transform.localScale.z;
                                // ���� �ϴ� ���̱�
                                outLineCloudRange.y += partnerCloud[1].transform.localScale.z;
                            }
                            break;
                        // ���� �Ʒ����� x �� �������� ������ ���ʿ��� ���������� �������� �����ؾ� �Ѵ�.
                        case MagneticCloudPos.MagneticCloudPos_Top:
                        case MagneticCloudPos.MagneticCloudPos_Bottom:
                            outLineCloudRange.x = this.transform.position.x - Mathf.Abs(this.transform.localScale.x) / 2;
                            outLineCloudRange.y = this.transform.position.x + Mathf.Abs(this.transform.localScale.x) / 2;
                            {// �ߺ��ڵ�
                             // ��� ���� ���̱�
                                outLineCloudRange.x += partnerCloud[0].transform.localScale.x;
                                // ��� ������ ���̱�
                                outLineCloudRange.y -= partnerCloud[1].transform.localScale.x;
                            }
                            break;
                    }
                    prevScale = this.transform.localScale;

                }
                switch (cloudType)
                {
                    // ������ z �� �������� ������ ������ �������� �����ؾ� �Ѵ�.
                    // �׷��ٸ� ������ ���� x ���� �����Ǿ� ���� �� �̴�.
                    case MagneticCloudPos.MagneticCloudPos_Left:
                    case MagneticCloudPos.MagneticCloudPos_Right:
                        {
                            float spacingValue = Random.Range(-cloudSpacing, cloudSpacing);
                            for (float z = outLineCloudRange.x; z > outLineCloudRange.y; z -= cloudSpacing)
                            {
                                GameObject cloud = ObjectPool.Instance.GetObjectInPool(CLIENTOBJ.CLIENTOBJ_CLOUDEFFECT, null
                                                                        , new Vector3(this.transform.position.x + (Mathf.Abs(this.transform.localScale.x) / 2) * (cloudType == MagneticCloudPos.MagneticCloudPos_Right ? -1 : 1)
                                                                        , this.transform.position.y + 0.5f
                                                                        , z + spacingValue));
                                cloud.GetComponent<CloudeEffect>().StartEffect();
                            }
                        }
                        break;
                    case MagneticCloudPos.MagneticCloudPos_Top:
                    case MagneticCloudPos.MagneticCloudPos_Bottom:
                        {
                            float spacingValue = Random.Range(-cloudSpacing, cloudSpacing);
                            for (float x = outLineCloudRange.x; x < outLineCloudRange.y; x += cloudSpacing)
                            {
                                //GameObject cloud = Instantiate(magneticCloud, this.transform.position, this.transform.rotation);
                                GameObject cloud = ObjectPool.Instance.GetObjectInPool(CLIENTOBJ.CLIENTOBJ_CLOUDEFFECT, null
                                                                       , new Vector3(x + spacingValue, this.transform.position.y + 0.5f, this.transform.position.z + (Mathf.Abs(this.transform.localScale.z) / 2) * (cloudType == MagneticCloudPos.MagneticCloudPos_Top ? -1 : 1)));
                                cloud.GetComponent<CloudeEffect>().StartEffect();
                            }
                            break;
                        }
                }
            yield return cloudSpacingTime;
        }
    }

    private void SetCloudRange()
    {
        
        randomXRange.x = this.transform.position.x - Mathf.Abs(this.transform.localScale.x) / 2;
        randomXRange.y = this.transform.position.x + Mathf.Abs(this.transform.localScale.x) / 2;
        randomZRange.x = this.transform.position.z - Mathf.Abs(this.transform.localScale.z) / 2;
        randomZRange.y = this.transform.position.z + Mathf.Abs(this.transform.localScale.z) / 2;

        switch (cloudType)
        {
            case MagneticCloudPos.MagneticCloudPos_Left:
                randomZRange.y -= partnerCloud[0].transform.localScale.z;
                break;
            case MagneticCloudPos.MagneticCloudPos_Right:
                randomZRange.x += partnerCloud[1].transform.localScale.z;
                break;
            case MagneticCloudPos.MagneticCloudPos_Bottom:
                randomXRange.x += partnerCloud[0].transform.localScale.x;
                break;
            case MagneticCloudPos.MagneticCloudPos_Top:
                randomXRange.y -= partnerCloud[1].transform.localScale.x;
                break;
        }

    }



    IEnumerator CreateRandomCloud()
    {
        while (true)
        {
            SetCloudRange();
            // �����Ͽ� ���� ���� ����
            float nowScale = Mathf.Abs(randomXRange.y - randomXRange.x) * Mathf.Abs(randomZRange.y - randomZRange.x);

            int realCreateCount = Mathf.RoundToInt((float)createCloudCount * (nowScale / originalScale));

            for (int i = 0; i < realCreateCount; ++i)
            {
                Vector3 randomPosition = new Vector3(Random.Range(randomXRange.x, randomXRange.y), this.transform.position.y + 0.5f, Random.Range(randomZRange.x, randomZRange.y));
                GameObject cloud = ObjectPool.Instance.GetObjectInPool(CLIENTOBJ.CLIENTOBJ_CLOUDEFFECT, null, randomPosition);
                cloud.GetComponent<CloudeEffect>().StartEffect();
            }
            yield return cloudRandomSpacingTime;
        }
    }
}
