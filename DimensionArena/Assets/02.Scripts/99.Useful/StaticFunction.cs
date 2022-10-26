using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticFunction
{
    public static T ChangeAlpha<T>(this T g, float newAlpha)
        where T : Material
    {
        var color = g.color;
        color.a = newAlpha;
        g.color = color;
        return g;
    }
}
