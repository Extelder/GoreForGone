using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ConsumableAnimator : ItemAnimator
{
    [SerializeField] private string _activateBoolName;

    private int _currentAttackInt;

    public override void DisableAllBools()
    {
        base.DisableAllBools();
        SetAnimationBool(_activateBoolName, false);
    }

    public void Activate()
    {
        SetAnimationBoolAndDisableOther(_activateBoolName);
    }
    
}