using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldRing : PlayerRing
{
    [SerializeField] private GameObject _shieldGFX;

    protected override void OnRingAbilityBindStarted(InputAction.CallbackContext obj)
    {
        _shieldGFX.SetActive(true);
    }

    protected override void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
        _shieldGFX.SetActive(false);
    }

    protected override void CancelAction()
    {
        _shieldGFX.SetActive(false);
    }
}