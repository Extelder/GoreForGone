using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordStateMachine : ItemStateMachine
{
    [SerializeField] private SwordState _attackState;
    [SerializeField] private SwordState _chargeAttackState;

    [SerializeField] private float _waitForStartChargingTime;

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

    protected override void OnPlayerStarted()
    {
        base.OnPlayerStarted();
        playerCharacter.Binds.Character.MainShoot.started += OnMainShootStarted;
        playerCharacter.Binds.Character.MainShoot.canceled += OnMainShootCanceled;
    }


    protected override void OnDisableVirtual()
    {
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