using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsumableStateMachine : ItemStateMachine
{
    [SerializeField] private ConsumableState _activateState;

    [SerializeField] private bool _initialized = true;

    public override void OnInitializeted()
    {
    }

    public void ActivateState()
    {
        ChangeState(_activateState);
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
        playerCharacter.Binds.Character.MainShoot.performed += OnActivatePerformed;
    }

    private void OnActivatePerformed(InputAction.CallbackContext obj)
    {
        ActivateState();
    }


    protected override void OnDisableVirtual()
    {
        if (!base.IsOwner)
            return;
        playerCharacter.Binds.Character.MainShoot.performed -= OnActivatePerformed;
    }
}