using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SwordAnimator : ItemAnimator
{
    [SerializeField] private string _attackBoolName;
    [SerializeField] private string _randomAttackIntName;
    [SerializeField] private string _chargingAttackBoolName;
    [SerializeField] private string _chargeAttackBoolName;
    [SerializeField] private string _hittedObjectBool;

    private int _currentAttackInt;

    public override void DisableAllBools()
    {
        base.DisableAllBools();
        SetAnimationBool(_attackBoolName, false);
        SetAnimationBool(_chargingAttackBoolName, false);
        SetAnimationBool(_chargeAttackBoolName, false);
    }

    public void RandomizeAtackAnimation()
    {
        _currentAttackInt = _currentAttackInt == 1 ? 0 : 1;
        SetAnimationInt(_randomAttackIntName, _currentAttackInt);
    }

    public void SetHittedObjectBool(bool value)
    {
        SetAnimationBool(_hittedObjectBool, value);
    }

    public void Attack()
    {
        RandomizeAtackAnimation();
        SetAnimationBoolAndDisableOther(_attackBoolName);
    }

    public void StartCharge()
    {
        SetAnimationBoolAndDisableOther(_chargingAttackBoolName);
    }

    public void ChargeAttack()
    {
        SetAnimationBoolAndDisableOther(_chargeAttackBoolName);
    }

    public void StopCharging()
    {
        SetAnimationBool(_chargingAttackBoolName, false);
    }
}