using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition 
{
    public IState ownerState;
    public IState targetState;
    List<Condition> conditions;


    public Transition(IState ownerState, IState targetState, List<Condition> conditions)
    {
        this.ownerState = ownerState;
        this.targetState = targetState;
        this.conditions = conditions;
    }

    public bool CheckChangeState()
    {
        foreach(var condition in conditions)
        {
            if (!condition.CheckCondition())
                return false;
        }

        return true;
    }
}
