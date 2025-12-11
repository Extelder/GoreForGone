using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAnimator : UnitAnimator
{
    [field: SerializeField] public string MoveAnimationBoolName, RunAnimationBoolName, AttackAnimationBoolName, ReactAnimationBoolName;

    public override void DisableAllBools()
    {
        SetAnimationBool(MoveAnimationBoolName, false);
        SetAnimationBool(AttackAnimationBoolName, false);
        SetAnimationBool(RunAnimationBoolName, false);
        SetAnimationBool(ReactAnimationBoolName, false);
    }

    public virtual void Move()
    {
        SetAnimationBoolAndDisableOther(MoveAnimationBoolName);
    }

    public virtual void Run()
    {
        SetAnimationBoolAndDisableOther(RunAnimationBoolName);
    }

    public virtual void React()
    {
        SetAnimationBoolAndDisableOther(ReactAnimationBoolName);
    }
    
    public virtual void Attack()
    {
        SetAnimationBoolAndDisableOther(AttackAnimationBoolName);
    }
}