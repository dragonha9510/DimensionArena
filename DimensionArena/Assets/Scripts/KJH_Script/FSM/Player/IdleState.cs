using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    Transition[] transitions;

    public IdleState()
    {
        List<Condition> runCondition = new List<Condition>();
        runCondition.Add(new Condition(1, 2, ConditionPredicate.CoditionCheckGreater));
        transitions[0] = new Transition(this, new RunState(), runCondition);
    }
    public void OnEnter()
    {
        
    }
    public void Start()
    {
        
    }
    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        
    }

    public void LateUpdate()
    {
       for(int i = 0; i < transitions.Length; ++i)
       {
            if (transitions[i].CheckChangeState())
                OnExit();
       }
    }

    public void OnExit()
    {
        
    }


}
