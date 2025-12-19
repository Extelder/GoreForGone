using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordBlockState : SwordState
{
    [SerializeField] private PlayerCheckOnEnemy _playerCheckOnEnemy;
    [SerializeField] private float _blockDamageMultiplier;
    private bool _enemyDetected;
    private float _defaultDamageMultiplier;
    private EnemyStateMachine _parriableEnemyStateMachine;
    private PlayerHitBox _playerHitBox;
    private bool _succesfullyParried;
    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void OnStartClient()
    {
        base.OnStartClient();
        _playerHitBox = PlayerCharacter.Instance.PlayerHitBox;
        _playerHitBox.TryParryAttack += OnAttackTriedParry;
    }

    private void OnAttackTriedParry(bool value, EnemyStateMachine parriableEnemy)
    {
        _disposable.Clear();
        Observable.Interval(TimeSpan.FromSeconds(0.02f)).Subscribe(_ =>
        {
            _enemyDetected = _playerCheckOnEnemy.EnemyDetected();
        }).AddTo(_disposable);
        _parriableEnemyStateMachine = parriableEnemy;
        _succesfullyParried = value;
    }

    public override void Enter()
    {
        _defaultDamageMultiplier = _playerHitBox.DamageMultiplier;
        Animator.Block();
        CanChanged = false;
        PlayerCharacter.Instance.Binds.Character.Block.canceled += OnBlockCancelled;
        Debug.Log(_succesfullyParried + "Parried");
        if (_succesfullyParried)
        {
            Debug.Log(_enemyDetected + "Detected");
            if (_enemyDetected)
            {
                _playerHitBox.DamageMultiplier = 0;
                _parriableEnemyStateMachine.React();
            }
            return;
        }
        _playerHitBox.DamageMultiplier = _blockDamageMultiplier;
    }

    public override void Exit()
    {
        _disposable.Clear();
        PlayerCharacter.Instance.Binds.Character.Block.canceled -= OnBlockCancelled;
        _playerHitBox.DamageMultiplier = _defaultDamageMultiplier;
    }

    private void OnDisable()
    {
        PlayerCharacter.Instance.Binds.Character.Block.canceled -= OnBlockCancelled;
        _disposable.Clear();
        _playerHitBox.TryParryAttack -= OnAttackTriedParry;
        _playerHitBox.DamageMultiplier = _defaultDamageMultiplier;
    }

    private void OnBlockCancelled(InputAction.CallbackContext obj)
    {
        CanChanged = true;
    }
}