using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerSpace;

public class JooHyeok_Skill : Player_Skill
{
    [SerializeField] private GameObject skillPrefab;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    public override void UseSkill(Vector3 direction, float magnitude)
    {
        //���� �Ÿ����� ���س����.. �� ������ ã�� �����Կ�..
        Debug.Log("������ ��ų ���� : " + direction);
        Debug.Log("������ ��ų �Ÿ� : " + magnitude);
        Debug.Log("������ ��ų ����");
    }


    private void ThrowGranade(Vector3 direction, float magnitude)
    {
        
    }
}
