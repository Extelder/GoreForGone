using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimator : UnitMultiAnimator
{
    [SerializeField] private string _inspectBool;
    [SerializeField] private string _moveBool;

    public override void DisableAllBools()
    {
        SetAnimationBool(_moveBool, false);
        SetAnimationBool(_inspectBool, false);
    }

    public void Inspect()
    {
        SetAnimationBoolAndDisableOther(_inspectBool);
    }

    public void Move()
    {
        SetAnimationBoolAndDisableOther(_moveBool);
    }
}