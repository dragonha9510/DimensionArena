using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimation : MonoBehaviour
{
    [SerializeField] private float interval;
    [SerializeField] private float rotSpeed;
    [SerializeField] private int rotCnt;

    private int curRotCnt;
    private bool direction;
    private float curZRot;
    private float oriZRot;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        oriZRot = rectTransform.rotation.z;
        
    }
  

    public IEnumerator Animation()
    {
        while(true)
        {
            float nowDirection = direction ? -1 : 1;

            curZRot += Time.deltaTime * nowDirection * rotSpeed;

            rectTransform.rotation = Quaternion.Euler(Vector3.forward * curZRot);

            if(curRotCnt == rotCnt)
            {
                if(direction)
                {
                    if (curZRot <= oriZRot)
                    {
                        curZRot = oriZRot;
                        rectTransform.rotation = Quaternion.Euler(Vector3.forward * curZRot);
                        break;
                    }
                }
                else
                {
                    if(curZRot >= oriZRot)
                    {
                        curZRot = oriZRot;
                        rectTransform.rotation = Quaternion.Euler(Vector3.forward * curZRot);
                        break;
                    }
                }
            }
            else if(Mathf.Abs(curZRot) >= interval)
            {
                curZRot = interval * nowDirection;
                direction = !direction;
                ++curRotCnt;
            }

            yield return null;
        }

        curRotCnt = 0;
        yield return null;
    }
}
