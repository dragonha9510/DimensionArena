using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyMove : MonoBehaviour
{
    public void MoveToZero()
    {
        this.transform.DOMove(new Vector3(0, 0, 0f), 1.0f);
    }
}
