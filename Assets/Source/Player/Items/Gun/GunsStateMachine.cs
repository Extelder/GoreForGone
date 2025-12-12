using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunsStateMachine : ItemStateMachine
{
    [SerializeField] private GunState _attackState;
    [SerializeField] private GunState _reloadState;

    private bool _initialized;

    public override void OnInitializeted()
    {
    }

    public void AttackState()
    {
        ChangeState(_attackState);
    }
    public void ReloadState()
    {
        ChangeState(_reloadState);
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
        playerCharacter.Binds.Character.MainShoot.performed += OnMainShootPerformed;
        playerCharacter.Binds.Character.Reload.performed += OnReloadPerformed;
    }

    private void OnReloadPerformed(InputAction.CallbackContext obj)
    {
        ReloadState();
    }


    protected override void OnDisableVirtual()
    {
        if (!base.IsOwner)
            return;
        playerCharacter.Binds.Character.MainShoot.performed -= OnMainShootPerformed;
        playerCharacter.Binds.Character.Reload.performed -= OnReloadPerformed;
    }

    private void OnMainShootPerformed(InputAction.CallbackContext obj)
    {
        StopAllCoroutines();
        AttackState();
    }
}