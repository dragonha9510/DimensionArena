using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithTime_Alpha : DestroyWithTime
{
    public float delayTime = 1f;
    private float maxTime;

    [SerializeField] private Renderer alphaRenderer;

    private void Start()
    {
        maxTime = base.deathTime + delayTime;
    }

    // Update is called once per frame
    override protected void Update()
    {
        curTime += Time.deltaTime;

        if (curTime >= deathTime)
        {
            alphaRenderer.material.color = 
                new Color(
                    alphaRenderer.material.color.r, 
                    alphaRenderer.material.color.g,
                    alphaRenderer.material.color.b,
                    (delayTime - (curTime - deathTime)) / delayTime);

            if (curTime >= maxTime)
                Destroy(this.gameObject);
        }
    }
}
