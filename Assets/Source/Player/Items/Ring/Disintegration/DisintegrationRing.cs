using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisintegrationRing : PlayerRing
{
    [SerializeField] private GameObject _shieldGFX;

    [SerializeField] private GameObject _shieldTPS;

    [SerializeField] private PlayerCheckOnEnemy _playerCheckOnEnemy;
    [SerializeField] private float _checkRate = 0.02f;

    private float _defaultDamageMultiplier;
    private bool _enemyDetected;
    private PlayerHitBox _playerHitBox;
    private CompositeDisposable _disposable = new CompositeDisposable();

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

        Observable.Interval(TimeSpan.FromSeconds(_checkRate)).Subscribe(_ =>
        {
            if (!_playerCheckOnEnemy.EnemyDetected())
            {
                _playerHitBox.DamageMultiplier = _defaultDamageMultiplier;
                return;
            }

            _playerHitBox.DamageMultiplier = 0;
        }).AddTo(_disposable);
    }

    protected override void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
        if (!base.IsOwner)
            return;

        _disposable.Clear();

        _shieldGFX.SetActive(false);
        PlayerCharacter.Instance.SetObjectEnableServer(_shieldTPS, false);
        _playerHitBox.DamageMultiplier = _defaultDamageMultiplier;
    }

    protected override void CancelAction()
    {
        if (!base.IsOwner)
            return;

        _disposable.Clear();

        _shieldGFX.SetActive(false);
        PlayerCharacter.Instance.SetObjectEnableServer(_shieldTPS, false);
        _playerHitBox.DamageMultiplier = _defaultDamageMultiplier;
    }
}