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

    // ���� ���� ���� �Դϴ� . ( ���� ������ )
    [SerializeField]
    private float cloudSpacing;
    [SerializeField]
    private WaitForSeconds cloudSpacingTime = new WaitForSeconds(1.0f);

    [SerializeField]
    private GameObject magneticCloud;

    private Bounds colliderBounds;

    private Vector2 outLineCloudRange;

    private DelayMachine delayMachine = new DelayMachine();

    // ���� ���� Ÿ���Դϴ� , �����¿� �Դϴ�.
    public MagneticCloudPos cloudType;

    private void SettingCloudLine()
    {

        switch (cloudType)
        {
            // ���� �������� z �� �������� ������ ������ �������� �����ؾ� �Ѵ�.
            case MagneticCloudPos.MagneticCloudPos_Left:
            case MagneticCloudPos.MagneticCloudPos_Right:
                outLineCloudRange.x = this.transform.position.z + colliderBounds.size.z / 2;
                outLineCloudRange.y = this.transform.position.z - colliderBounds.size.z / 2;
                break;
            // ���� �Ʒ����� x �� �������� ������ ���ʿ��� ���������� �������� �����ؾ� �Ѵ�.
            case MagneticCloudPos.MagneticCloudPos_Top:
            case MagneticCloudPos.MagneticCloudPos_Bottom:
                outLineCloudRange.x = this.transform.position.x - colliderBounds.size.x / 2;
                outLineCloudRange.y = this.transform.position.x + colliderBounds.size.x / 2;
                break;
        }
    }
    IEnumerator CreateCloud()
    {
        while(true)
        {
            switch (cloudType)
                {
                    // ������ z �� �������� ������ ������ �������� �����ؾ� �Ѵ�.
                    // �׷��ٸ� ������ ���� x ���� �����Ǿ� ���� �� �̴�.
                    case MagneticCloudPos.MagneticCloudPos_Left:
                    case MagneticCloudPos.MagneticCloudPos_Right:
                        for (float z = outLineCloudRange.x; z > outLineCloudRange.y; z -= cloudSpacing)
                        {
                            Debug.Log("�� �� ��������");
                            
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
                            Debug.Log("�� �� ��������");
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

    void Start()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        colliderBounds = collider.bounds;
        SettingCloudLine();
        StartCoroutine("CreateCloud");
    }

    // Update is called once per frame
    void Update()
    {
    }


}
