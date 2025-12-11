using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordState : State
{
    [field: SerializeField] public SwordAnimator Animator { get; private set; }

    public override void Enter()
    {
    }
}