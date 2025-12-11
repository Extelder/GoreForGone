using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnimator : ItemAnimator
{
    [SerializeField] private string _attackBoolName;
    [SerializeField] private string _chargingAttackBoolName;
    [SerializeField] private string _chargeAttackTriggerName;

    public override void DisableAllBools()
    {
        base.DisableAllBools();
        SetAnimationBool(_attackBoolName, false);
        SetAnimationBool(_chargingAttackBoolName, false);
        ResetAnimationTrigger(_chargeAttackTriggerName);
    }

    public void Attack()
    {
        SetAnimationBoolAndDisableOther(_attackBoolName);
    }

    public void StartCharge()
    {
        SetAnimationBoolAndDisableOther(_chargingAttackBoolName);
    }

    public void ChargeAttack()
    {
        SetAnimationTrigger(_chargeAttackTriggerName);
    }

    public void StopCharging()
    {
        SetAnimationBool(_chargingAttackBoolName, false);
    }
}