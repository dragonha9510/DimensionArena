using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public event Action<float> skillAmountChanged;
    [SerializeField]int maxHP;
    [SerializeField]int hp;
    [SerializeField]int curSkillPoint;
    [SerializeField]int maxSkillPoint;

    private void Start()
    {
    }



    public void GetSkillPoint(int point)
    {
        curSkillPoint += point;
        Mathf.Clamp(curSkillPoint, 0, maxSkillPoint);
        skillAmountChanged((float)curSkillPoint / (float)maxSkillPoint);
    }

    public void Damaged(int damage)
    {
        hp -= damage;
        Mathf.Clamp(hp, 0, maxHP);
    }

}
