using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldRing : PlayerRing
{
    [SerializeField] private GameObject _shieldGFX;
    private float _defaultDamageMultiplier;
    private PlayerHitBox _playerHitBox;

    public override void OnStartClient()
    {
        base.OnStartClient();
        _playerHitBox = PlayerCharacter.Instance.PlayerHitBox;
    }

    protected override void OnRingAbilityBindStarted(InputAction.CallbackContext obj)
    {
        _defaultDamageMultiplier = _playerHitBox.DamageMultiplier;
        _shieldGFX.SetActive(true);
        _playerHitBox.DamageMultiplier = 0;
    }

    protected override void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
        _shieldGFX.SetActive(false);
        _playerHitBox.DamageMultiplier = _defaultDamageMultiplier;
    }

    protected override void CancelAction()
    {
        _shieldGFX.SetActive(false);
        _playerHitBox.DamageMultiplier = _defaultDamageMultiplier;
    }
}