using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerRing : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _character;

    private PlayerBinds _binds;

    private void Awake()
    {
        _character.ClientStarted += OnClientStarted;
    }

    private void OnClientStarted()
    {
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
        _binds.Character.RingAbility.started -= OnRingAbilityBindStarted;
        _binds.Character.RingAbility.canceled -= OnRingAbilityBindCanceled;
        _binds.Character.RingAbility.performed -= OnRingAbilityBindPerformed;


        _character.ClientStarted -= OnClientStarted;
        CancelAction();
        _binds.Character.CancelAction.started -= OnCancelActionStarted;
    }
}