using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimator : UnitMultiAnimator
{
    [SerializeField] private string _moveBool;

    public override void DisableAllBools()
    {
        SetAnimationBool(_moveBool, false);
    }

    public void Move()
    {
        SetAnimationBoolAndDisableOther(_moveBool);
    }
}