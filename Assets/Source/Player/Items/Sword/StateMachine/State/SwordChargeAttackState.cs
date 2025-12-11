using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordChargeAttackState : SwordState
{
    private CompositeDisposable _disposable = new CompositeDisposable();

    private bool _charged = false;

    public override void Enter()
    {
        _charged = false;

        CanChanged = false;
        Animator.StartCharge();
        PlayerCharacter.Instance.Binds.Character.MainShoot.canceled += OnMainShootCanceled;
    }

    private void OnDisable()
    {
        PlayerCharacter.Instance.Binds.Character.MainShoot.canceled -= OnMainShootCanceled;
        _disposable.Clear();
    }

    public void ChargeCompleate()
    {
        _charged = true;
    }

    public void OnChargeAttackAnimationEnded()
    {
        Animator.DisableAllBools();
        CanChanged = true;
    }

    private void OnMainShootCanceled(InputAction.CallbackContext obj)
    {
        PlayerCharacter.Instance.Binds.Character.MainShoot.canceled -= OnMainShootCanceled;
        if (_charged)
        {
            Animator.ChargeAttack();
        }
        else
        {
            CanChanged = true;
        }
    }
}