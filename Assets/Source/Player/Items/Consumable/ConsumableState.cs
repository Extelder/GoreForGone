using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableState : State
{
    [field: SerializeField] public ConsumableAnimator Animator { get; private set; } 
    public override void Enter()
    {
        
    }
    
    
}