using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartScript : MonoBehaviour
{
    //PartName
    public string PartName = "Empty";
    //Grid Map Idx
    public int Row, Column;
    
    [HideInInspector] public GUIStyle style;
}
