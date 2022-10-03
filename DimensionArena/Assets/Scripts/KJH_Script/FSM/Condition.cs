using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class ConditionPredicate
{

    public static bool CoditionCheckGreater(object target, object greaterValue)
    {
        if ((float)target >= (float)greaterValue)
            return true;

        return false;
    }

    public static bool CoditionCheckLess(object target, object lessValue)
    {
        if ((float)target < (float)lessValue)
            return true;

        return false;
    }


    public static bool CoditionCheckEqual(object target, object bBool)
    {
        if ((bool)target == (bool)bBool)
            return true;

        return false;
    }
}

 
public class Condition 
{

    public object target;
    public object predicateObj;
    public Func<object, object, bool> predicate;

    public Condition(object target, object preficateObj, Func<object, object, bool> predicate)
    {
        this.target = target;
        this.predicateObj = preficateObj;
        this.predicate = predicate;
    }


    public bool CheckCondition()
    {
        if(predicate(target,predicateObj))
        {
            return true;
        }

        return false;
    }
}
