using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{
    private static FloatingText instance;


    public static FloatingText Instance
    {
        get
        {
            if (!instance)
            {
                if (!(instance = GameObject.FindObjectOfType<FloatingText>()))
                {
                    GameObject obj = new GameObject("FloatingTextParent");
                    instance = obj.AddComponent<FloatingText>();
                }
            }

            return instance;
        }

    }


    [SerializeField] GameObject floatingPrefab;

    [Header("±‚»π/Floating Time")]
    [SerializeField] private float floatTime;
    [SerializeField] private float floatRange;


    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
    }

    public void CreateFloatingTextForDamage(Vector3 position, float Damage)
    {
        GameObject floatingObj = Instantiate(floatingPrefab, position + Vector3.up, Quaternion.identity, transform);
        TextMesh textMesh = floatingObj.GetComponent<TextMesh>();
        textMesh.text = Damage.ToString();
        StartCoroutine(FloatingAnimationCoroutine(floatingObj));
    }



    IEnumerator FloatingAnimationCoroutine(GameObject obj)
    {
        obj.transform.DOMoveY(obj.transform.position.y + floatRange, floatTime);
        WaitForSeconds floatDelay = new WaitForSeconds(floatTime * 0.01f);
        TextMesh textMesh = obj.GetComponent<TextMesh>();
        yield return new WaitForSeconds(floatTime);

        for (int i = 0; i < 100; ++i)
        {
            textMesh.color = new Color(1, 1, 1, 1 - i * 0.01f);
            yield return floatDelay;
        }

        Destroy(obj);
    }


}
