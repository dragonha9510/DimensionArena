using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackObject : MonoBehaviour
{
    [SerializeField] private KnockBackInfo info;
    [SerializeField] private GameObject knockBack;
    [SerializeField] private float triggerRadius;
    private GameObject temp;

    [SerializeField] private bool isOnDestroy = true;
    [SerializeField] private bool isOnDisable;
    [SerializeField] private bool isOnEnable;

    // Start is called before the first frame update
    void Start()
    {
        temp = Instantiate(knockBack, transform.position, knockBack.transform.rotation);
        temp.GetComponent<KnockBack>().info = info;
        temp.SetActive(false);

        if (isOnEnable)
            this.enabled = false;
    }

    void KnockBackStart()
    {
        temp.transform.position = transform.position;
        temp.SetActive(true);
    }

    private void OnDestroy()
    {
        if (!isOnDestroy)
            return;

        KnockBackStart();
    }

    private void OnDisable()
    {
        if (!isOnDisable)
            return;

        KnockBackStart();
    }

    private void OnEnable()
    {
        if (!isOnEnable)
            return;

        KnockBackStart();
        this.enabled = false;
    }
}
