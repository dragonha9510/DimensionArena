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
        //방향 거리까진 구해놨어요.. 전 자유를 찾아 떠날게요..
        Debug.Log("주혁이 스킬 방향 : " + direction);
        Debug.Log("주혁이 스킬 거리 : " + magnitude);
        Debug.Log("주혁이 스킬 시작");
    }


    private void ThrowGranade(Vector3 direction, float magnitude)
    {
        
    }
}
