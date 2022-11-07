using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackObject : MonoBehaviour
{
    [SerializeField] private KnockBackInfo info;
    [SerializeField] private GameObject knockBack;
    [SerializeField] private float triggerRadius;
    private GameObject temp;
    // Start is called before the first frame update
    void Start()
    {
        temp = Instantiate(knockBack, transform.position, knockBack.transform.rotation);
        temp.GetComponent<KnockBack>().info = info;
        temp.SetActive(false);
    }

    public void KnockBackStart()
    {
        temp.transform.position = transform.position;
        temp.SetActive(true);
    }


    private void OnDestroy()
    {
        //KnockBackStart();
    }

    private void OnDisable()
    {
        //KnockBackStart();
    }
}
