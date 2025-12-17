using System;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerRing : NetworkBehaviour
{
    [SerializeField] private PlayerCharacter _character;

    private PlayerBinds _binds;

    [SerializeField] private bool _initialized;

    private void Awake()
    {
        _character.ClientStarted += OnClientStarted;
    }

    private void OnEnable()
    {
        if (!_initialized)
            return;
        if (!base.IsOwner)
            return;
        OnClientStarted();
    }

    private void OnClientStarted()
    {
        if (!base.IsOwner)
            return;
        _initialized = true;
        _binds = PlayerCharacter.Instance.Binds;

        _binds.Character.RingAbility.started += OnRingAbilityBindStarted;
        _binds.Character.RingAbility.canceled += OnRingAbilityBindCanceled;
        _binds.Character.RingAbility.performed += OnRingAbilityBindPerformed;

        _binds.Character.CancelAction.started += OnCancelActionStarted;
    }


    private void OnCancelActionStarted(InputAction.CallbackContext obj)
    {
        CancelAction();
    }

    protected abstract void CancelAction();

    protected virtual void OnRingAbilityBindPerformed(InputAction.CallbackContext obj)
    {
    }

    protected virtual void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
    }

    protected virtual void OnRingAbilityBindStarted(InputAction.CallbackContext obj)
    {
    }


    private void OnDisable()
    {
        if (!base.IsOwner)
            return;
        _binds.Character.RingAbility.started -= OnRingAbilityBindStarted;
        _binds.Character.RingAbility.canceled -= OnRingAbilityBindCanceled;
        _binds.Character.RingAbility.performed -= OnRingAbilityBindPerformed;


        _character.ClientStarted -= OnClientStarted;
        CancelAction();
        _binds.Character.CancelAction.started -= OnCancelActionStarted;
    }
}