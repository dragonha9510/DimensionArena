using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraSkillProjectile : Projectile
{
    [SerializeField]
    private GameObject explosionEffect;
    [SerializeField]
    private float createCycle = 0.3f;
    private float timer = 0f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartCoroutine(CreateEffect());
        timer = createCycle;
    }

    IEnumerator CreateEffect()
    {
        while(true)
        {
            timer += Time.deltaTime;
            if(timer >= createCycle)
            {
                SoundManager.Instance.PlaySFXOneShot(hitAudioClipName);
                Instantiate(explosionEffect,this.transform.position,Quaternion.identity);
                timer = 0f;
            }
            yield return null;
        }
    }

}
