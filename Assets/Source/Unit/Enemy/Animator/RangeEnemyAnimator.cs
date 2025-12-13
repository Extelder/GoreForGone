using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyAnimator : EnemyAnimator
{
    [SerializeField] private string _secondAttackAnimationBool, _idleAnimationBool, _idleAnimationInt;
    [SerializeField] private int _maxIdleVariantsCount;
    
    public override void DisableAllBools()
    {
        base.DisableAllBools();
        SetAnimationBool(_idleAnimationBool, false);
    }
    
    public override void Idle()
    {
        SetAnimationInt(_idleAnimationInt, Random.Range(0, _maxIdleVariantsCount-1));
        SetAnimationBoolAndDisableOther(_idleAnimationBool);
    }

    public override void Shoot()
    {
        SetAnimationBoolAndDisableOther(_secondAttackAnimationBool);
    }
}
