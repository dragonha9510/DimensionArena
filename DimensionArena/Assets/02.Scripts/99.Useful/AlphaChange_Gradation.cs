using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaChange_Gradation : MonoBehaviour
{
    [SerializeField] private Vector2 alphaFromTo = new Vector2(1, 0);
    [SerializeField] private float Speed = 1f;

    private MeshRenderer _renderer;
    private ParticleSystemRenderer _renderer2;

    private Material material;
    private float alpha;
    private int isNegative;
    private bool isEnd;

    private void Initialize()
    {
        if (alphaFromTo.y - alphaFromTo.x == 0)
        {
            isEnd = true;
            return;
        }

        isNegative = (alphaFromTo.y - alphaFromTo.x) < 0 ? -1 : 1;

        _renderer = GetComponent<MeshRenderer>();
        
        if(_renderer == null)
        {
            _renderer2 = GetComponent<ParticleSystemRenderer>();

            if (_renderer2 != null)
                material = _renderer2.sharedMaterial;
        }
        else
            material = _renderer.sharedMaterial;

        alpha = alphaFromTo.x;
        material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);
    }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Update()
    {
        SetAlphaFromTo();
    }

    private void SetAlphaFromTo()
    {
        if (isEnd)
            return;

        alpha += Speed * Time.deltaTime * isNegative;
        StaticFunction.ChangeAlpha<Material>(material, alpha);       
        isEnd = (isNegative == 1 ? (alpha >= alphaFromTo.y) : (alpha <= alphaFromTo.y));
    }
}
