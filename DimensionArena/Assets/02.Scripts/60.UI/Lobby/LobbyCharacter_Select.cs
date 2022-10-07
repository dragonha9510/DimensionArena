using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyCharacter_Select : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject particle;
    [SerializeField] private float speed = 10.0f;
    private Vector3 oriScale;
    private bool isOn;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOn = true;
        particle.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOn = false;
        particle.transform.localScale = new Vector3(0, 0, 1);
        particle.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        oriScale = particle.transform.localScale;
        particle.SetActive(false);
        particle.transform.localScale = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(isOn && (particle.transform.localScale.x <= oriScale.x))
        {
            particle.transform.localScale += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0);
        }
    }
}
