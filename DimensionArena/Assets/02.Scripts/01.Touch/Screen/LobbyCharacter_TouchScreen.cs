using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyCharacter_TouchScreen : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform player;
    [SerializeField] private float rotationSpeed = 90.0f;
    private Vector3 oriRot;
    private Vector2 touchPos;
    private bool isTouch;
    private bool once;

    private void Start()
    {
        oriRot = player.rotation.eulerAngles;
        Debug.Log(oriRot);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (once)
        {
            StopCoroutine(RecoverRotation(0));
        }
        once = true;
        isTouch = true;
        touchPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        player.Rotate(new Vector3(0, (touchPos.x - eventData.position.x), 0));

        touchPos.x = eventData.position.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isTouch = false;

        StartCoroutine("RecoverRotation", (Time.deltaTime));
    }

    IEnumerator RecoverRotation(float delta)
    {
        while(!isTouch)
        {
            float curY = player.rotation.y * Mathf.Rad2Deg;

            if (Mathf.Abs(curY - oriRot.y) <= rotationSpeed * delta)
            {
                yield break;
            }

            float angle = player.rotation.eulerAngles.y < 180 ? -1 : 1;

            Debug.Log(angle);

            player.Rotate(new Vector3(0, (angle * rotationSpeed * delta), 0));
            yield return null;
        }

        yield break;
    }
}
