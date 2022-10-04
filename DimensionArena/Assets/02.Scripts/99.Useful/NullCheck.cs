using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NullCheck 
{
   public static bool IsNullOrEmpty<T>(List<T> array)
   {
        return (array == null || array.Count == 0);
   }

    public static bool IsNullOrEmpty<T>(T[] array)
    {
        return (array == null || array.Length == 0);
    }
}
