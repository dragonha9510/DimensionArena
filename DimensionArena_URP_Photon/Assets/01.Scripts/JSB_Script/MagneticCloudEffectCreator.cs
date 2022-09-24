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


    void Start()
    {
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
            // ���� �������� z �� �������� ������ ������ �������� �����ؾ� �Ѵ�.
            case MagneticCloudPos.MagneticCloudPos_Left:
            case MagneticCloudPos.MagneticCloudPos_Right:
                outLineCloudRange.x = this.transform.position.z + this.transform.localScale.z / 2;
                outLineCloudRange.y = this.transform.position.z - this.transform.localScale.z / 2;
                break;
            // ���� �Ʒ����� x �� �������� ������ ���ʿ��� ���������� �������� �����ؾ� �Ѵ�.
            case MagneticCloudPos.MagneticCloudPos_Top:
            case MagneticCloudPos.MagneticCloudPos_Bottom:
                outLineCloudRange.x = this.transform.position.x - this.transform.localScale.x / 2;
                outLineCloudRange.y = this.transform.position.x + this.transform.localScale.x / 2;
                break;
        }
    }
    IEnumerator CreateEdgeCloud()
    {
        while(true)
        {
            if (null != partnerCloud)
            {
                switch (cloudType)
                {
                    // ���� �������� z �� �������� ������ ������ �������� �����ؾ� �Ѵ�.
                    case MagneticCloudPos.MagneticCloudPos_Left:
                        outLineCloudRange.x = this.transform.position.z + this.transform.localScale.z / 2;
                        outLineCloudRange.y = this.transform.position.z - this.transform.localScale.z / 2;
                        {// �ߺ��ڵ�
                            // ���� ��� ���̱�
                            outLineCloudRange.x -= partnerCloud[0].transform.localScale.z;
                            // ���� �ϴ� ���̱�
                            outLineCloudRange.y += partnerCloud[1].transform.localScale.z;
                        }
                        break;
                    case MagneticCloudPos.MagneticCloudPos_Right:
                        outLineCloudRange.x = this.transform.position.z + this.transform.localScale.z / 2;
                        outLineCloudRange.y = this.transform.position.z - this.transform.localScale.z / 2;
                        {// �ߺ��ڵ�
                            // ���� ��� ���̱�
                            outLineCloudRange.x -= partnerCloud[0].transform.localScale.z;
                            // ���� �ϴ� ���̱�
                            outLineCloudRange.y += partnerCloud[1].transform.localScale.z;
                        }
                        break;
                    // ���� �Ʒ����� x �� �������� ������ ���ʿ��� ���������� �������� �����ؾ� �Ѵ�.
                    case MagneticCloudPos.MagneticCloudPos_Top:
                        outLineCloudRange.x = this.transform.position.x - this.transform.localScale.x / 2;
                        outLineCloudRange.y = this.transform.position.x + this.transform.localScale.x / 2;
                        {// �ߺ��ڵ�
                         // ��� ���� ���̱�
                            outLineCloudRange.x += partnerCloud[0].transform.localScale.x;
                            // ��� ������ ���̱�
                            outLineCloudRange.y -= partnerCloud[1].transform.localScale.x;
                        }

                        break;
                    case MagneticCloudPos.MagneticCloudPos_Bottom:
                        outLineCloudRange.x = this.transform.position.x - this.transform.localScale.x / 2;
                        outLineCloudRange.y = this.transform.position.x + this.transform.localScale.x / 2;
                        {// �ߺ��ڵ�
                         // ��� ���� ���̱�
                            outLineCloudRange.x += partnerCloud[0].transform.localScale.x;
                            // ��� ������ ���̱�
                            outLineCloudRange.y -= partnerCloud[1].transform.localScale.x;
                        }
                        break;
                }
            }
            switch (cloudType)
                {
                    // ������ z �� �������� ������ ������ �������� �����ؾ� �Ѵ�.
                    // �׷��ٸ� ������ ���� x ���� �����Ǿ� ���� �� �̴�.
                    case MagneticCloudPos.MagneticCloudPos_Left:
                    case MagneticCloudPos.MagneticCloudPos_Right:
                        for (float z = outLineCloudRange.x; z > outLineCloudRange.y; z -= cloudSpacing)
                        {
                            Debug.Log("���� ����");
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
                            Debug.Log("���� ����");

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
            // �����Ͽ� ���� ���� ����
            float nowScale = Mathf.Abs(this.transform.localScale.x) * Mathf.Abs(this.transform.localScale.z);

            int realCreateCount = Mathf.RoundToInt((float)createCloudCount * (nowScale / originalScale));
            for(int i = 0; i < realCreateCount; ++i)
            {
                Vector3 randomPosition = new Vector3(Random.Range(randomXRange.x, randomXRange.y), this.transform.position.y, Random.Range(randomZRange.x, randomZRange.y));
                GameObject cloud = Instantiate(magneticCloud);
                cloud.transform.position = randomPosition;
            }
            yield return cloudRandomSpacingTime;
        }
    }
}
