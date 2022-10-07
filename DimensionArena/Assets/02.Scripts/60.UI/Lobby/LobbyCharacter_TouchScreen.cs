using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyCharacter_TouchScreen : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Transform player;
    [SerializeField] private float rotationSpeed = 90.0f;
    private Vector3 oriRot;
    private Vector2 touchPos;
    private bool isTouch;
    private float curTouchTime;
    private float maxTouchTime;
    private bool once;

    private void Start()
    {
        oriRot = player.rotation.eulerAngles;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isTouch)
        {
            isTouch = false;
            // Scene 실행
            Debug.Log("Scene 전환!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (once)
            StopCoroutine("RecoverRotation");

        isTouch = false;
        once = true;
        touchPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        player.Rotate(new Vector3(0, (touchPos.x - eventData.position.x), 0));

        touchPos.x = eventData.position.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine("RecoverRotation", (Time.deltaTime));
    }

    IEnumerator RecoverRotation(float delta)
    {
        while(true)
        {
            float curY = player.rotation.y * Mathf.Rad2Deg;

            if (Mathf.Abs(curY - oriRot.y) <= rotationSpeed * delta)
                yield break;

            float angle = player.rotation.eulerAngles.y < 180 ? -1 : 1;
            float speedAccel = player.rotation.eulerAngles.y - (angle > 0 ? 360 : 0);
            speedAccel = Mathf.Abs(speedAccel) / 90;

            player.Rotate(new Vector3(0, (angle * (rotationSpeed * speedAccel) * delta), 0));

            yield return null;
        }
    }


}
