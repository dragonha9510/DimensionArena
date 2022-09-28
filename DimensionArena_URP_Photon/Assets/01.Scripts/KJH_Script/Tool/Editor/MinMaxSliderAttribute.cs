using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
public class MinMaxSliderAttribute : PropertyAttribute
{
    public float min;
    public float max;

    MinMaxSliderAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
