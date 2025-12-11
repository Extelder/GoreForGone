using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAnimator : EnemyAnimator
{
    [SerializeField] private string _reactAnimationBool, _idleAnimationBool, _attackAnimationInt, _idleAnimationInt;
    [SerializeField] private int _maxAttackVariantsCount;
    [SerializeField] private int _maxIdleVariantsCount;

    public override void DisableAllBools()
    {
        base.DisableAllBools();
        SetAnimationBool(_reactAnimationBool, false);
        SetAnimationBool(_idleAnimationBool, false);
    }

    public override void Idle()
    {
        SetAnimationInt(_idleAnimationInt, Random.Range(0, _maxIdleVariantsCount-1));
        SetAnimationBoolAndDisableOther(_idleAnimationBool);
    }

    public void React()
    {
        SetAnimationBoolAndDisableOther(_reactAnimationBool);
    }

    public override void Attack()
    {
        SetAnimationInt(_attackAnimationInt, Random.Range(0, _maxAttackVariantsCount-1));
        base.Attack();
    }
}
