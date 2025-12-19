using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldRing : PlayerRing
{
    [SerializeField] private GameObject _shieldGFX;
    [SerializeField] private GameObject _shieldTPS;
    private float _defaultDamageMultiplier;
    private PlayerHitBox _playerHitBox;

    public override void OnStartClient()
    {
        base.OnStartClient();
        _playerHitBox = PlayerCharacter.Instance.PlayerHitBox;
    }

    protected override void OnRingAbilityBindStarted(InputAction.CallbackContext obj)
    {
        if (!base.IsOwner)
            return;
        _defaultDamageMultiplier = _playerHitBox.DamageMultiplier;
        _shieldGFX.SetActive(true);
        PlayerCharacter.Instance.SetObjectEnableServer(_shieldTPS, true);
        _playerHitBox.DamageMultiplier = 0;
    }

    protected override void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
        if (!base.IsOwner)
            return;
        _shieldGFX.SetActive(false);
        PlayerCharacter.Instance.SetObjectEnableServer(_shieldTPS, false);
        _playerHitBox.DamageMultiplier = _defaultDamageMultiplier;
    }

    protected override void CancelAction()
    {
        if (!base.IsOwner)
            return;
        _shieldGFX.SetActive(false);
        PlayerCharacter.Instance.SetObjectEnableServer(_shieldTPS, false);
        _playerHitBox.DamageMultiplier = _defaultDamageMultiplier;
    }
}