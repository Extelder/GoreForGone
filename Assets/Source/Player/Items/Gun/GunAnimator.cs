using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunAnimator : ItemAnimator
{
    [SerializeField] private string _attackBoolName;
    [SerializeField] private string _reloadBoolName;

    private int _currentAttackInt;

    public override void DisableAllBools()
    {
        base.DisableAllBools();
        SetAnimationBool(_attackBoolName, false);
        SetAnimationBool(_reloadBoolName, false);
    }

    public void Attack()
    {
        SetAnimationBoolAndDisableOther(_attackBoolName);
    }

    public void Reload()
    {
        SetAnimationBoolAndDisableOther(_reloadBoolName);
    }
}