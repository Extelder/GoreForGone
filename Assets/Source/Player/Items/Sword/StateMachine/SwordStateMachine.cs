using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordStateMachine : ItemStateMachine
{
    [SerializeField] private SwordState _attackState;
    [SerializeField] private SwordState _chargeAttackState;

    [SerializeField] private float _waitForStartChargingTime;

    private bool _initialized;

    public override void OnInitializeted()
    {
    }

    public void AttackState()
    {
        ChangeState(_attackState);
    }

    public void ChargeAttackState()
    {
        CurrentState.CanChanged = true;
        ChangeState(_chargeAttackState);
    }

    private void OnEnable()
    {
        if (!_initialized)
            return;
        if (!base.IsOwner)
            return;
        OnPlayerStarted();
        CurrentState.CanChanged = true;
    }


    protected override void OnPlayerStarted()
    {
        if (!base.IsOwner)
            return;
        base.OnPlayerStarted();

        _initialized = true;
        playerCharacter.Binds.Character.MainShoot.started += OnMainShootStarted;
        playerCharacter.Binds.Character.MainShoot.canceled += OnMainShootCanceled;
    }


    protected override void OnDisableVirtual()
    {
        if (!base.IsOwner)
            return;
        playerCharacter.Binds.Character.MainShoot.started -= OnMainShootStarted;
        playerCharacter.Binds.Character.MainShoot.canceled -= OnMainShootCanceled;
    }

    private IEnumerator WaitingForStartCharging()
    {
        yield return new WaitForSeconds(_waitForStartChargingTime);
        ChargeAttackState();
    }

    private void OnMainShootCanceled(InputAction.CallbackContext obj)
    {
        StopAllCoroutines();
        AttackState();
    }

    private void OnMainShootStarted(InputAction.CallbackContext obj)
    {
        StopAllCoroutines();
        StartCoroutine(WaitingForStartCharging());
    }
}