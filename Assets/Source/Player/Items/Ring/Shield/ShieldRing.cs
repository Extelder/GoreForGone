using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldRing : PlayerRing
{
    [SerializeField] private GameObject _shieldGFX;
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
        _defaultDamageMultiplier = _playerHitBox.DamageMultiplier;
        _shieldGFX.SetActive(true);
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
        _disposable.Clear();
        _shieldGFX.SetActive(false);
        _playerHitBox.DamageMultiplier = _defaultDamageMultiplier;
    }

    protected override void CancelAction()
    {
        _disposable.Clear();
        _shieldGFX.SetActive(false);
        _playerHitBox.DamageMultiplier = _defaultDamageMultiplier;
    }
}