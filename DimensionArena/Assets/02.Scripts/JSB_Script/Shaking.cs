using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shaking : MonoBehaviour
{
    // ShakingLogic
    private bool isShaking = false;
    public bool IsShaking { get { return isShaking; } }
    [SerializeField]
    private float moveUnit = 0.01f;

    [SerializeField]
    private int moveCount = 2;
    private int nowMoveCount = 0;
    [SerializeField]
    private float maxRange;
    private float minPos;
    private float maxPos;

    private Vector3 originalPos;


    private bool Moving(Vector3 direction, float arriveX)
    {
        if (direction == Vector3.left)
        {
            if (arriveX >= this.transform.localPosition.x)
            {
                return true;
            }
            if (this.transform.localPosition.x < originalPos.x)
                --nowMoveCount;
        }
        else if (direction == Vector3.right)
        {
            if (arriveX <= this.transform.localPosition.x)
            {
                return true;
            }
            if (this.transform.localPosition.x > originalPos.x)
                --nowMoveCount;
        }

        this.transform.Translate(direction * moveUnit, Space.World);
        return false;
    }
    private bool MoveOriginPos(Vector3 arrivePos, bool moveLeft)
    {
        if (moveLeft && arrivePos.x >= this.transform.localPosition.x)
        {
            this.transform.localPosition = arrivePos;
            return true;
        }
        else if (!moveLeft && arrivePos.x <= this.transform.localPosition.x)
        {
            this.transform.localPosition = arrivePos;
            return true;
        }
        this.transform.Translate((arrivePos - this.transform.localPosition).normalized * moveUnit, Space.World);
        return false;
    }
    public void StartShaking()
    {
        //transform.DOShakePosition()
        StartCoroutine(nameof(ObjectShaking));
    }
    private IEnumerator ObjectShaking()
    {
        nowMoveCount = moveCount;
        isShaking = true;
        bool moveLeft = true;
        minPos = this.transform.localPosition.x - maxRange;
        maxPos = this.transform.localPosition.x + maxRange;
        originalPos = this.transform.localPosition;

        while (true)
        {
            if (0 < nowMoveCount)
            {
                switch (moveLeft)
                {
                    case true:
                        if (Moving(Vector3.left, minPos))
                        {
                            moveLeft = false;
                        }
                        break;
                    case false:
                        if (Moving(Vector3.right, maxPos))
                        {
                            moveLeft = true;
                        }
                        break;
                }
                yield return null;
            }
            else
            {
                if (MoveOriginPos(originalPos, moveLeft))
                {
                    isShaking = false;
                    yield break;
                }
                else
                    yield return null;

            }
        }
    }
}