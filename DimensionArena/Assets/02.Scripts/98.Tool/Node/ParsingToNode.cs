using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GRITTY;

public class ParsingToNode : MonoBehaviour
{
#if UNITY_EDITOR
    public Vector2Int idx;
    [HideInInspector] public Rect rect;
    public NodeInformation nodeInfo;
#endif

#if !UNITY_EDITOR
    private void Awake()
    {
        Destroy(this);   
    }
#endif

}


