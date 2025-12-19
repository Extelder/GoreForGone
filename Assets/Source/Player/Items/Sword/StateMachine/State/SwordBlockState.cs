using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordBlockState : SwordState
{
    [SerializeField] private PlayerHitBox _playerHitBox;
    [SerializeField] private PlayerCheckOnEnemy _playerCheckOnEnemy;
    [SerializeField] private float _blockDamageMultiplier;
    private bool _enemyDetected;
    private float _defaultDamageMultiplier;
    private EnemyStateMachine _parriableEnemyStateMachine;
    private bool _succesfullyParried;

    public override void OnStartClient()
    {
        base.OnStartClient();
        _playerHitBox.TryParryAttack += OnAttackTriedParry;
    }

    private void OnAttackTriedParry(bool value, EnemyStateMachine parriableEnemy)
    {
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
            Debug.Log(_playerCheckOnEnemy.EnemyDetected() + "Detected");
            if (_playerCheckOnEnemy.EnemyDetected())
            {
                _playerHitBox.DamageMultiplier = 0;
                _parriableEnemyStateMachine.React();
                return;
            }
        }
        _playerHitBox.DamageMultiplier = _blockDamageMultiplier;
    }

    public override void Exit()
    {
        PlayerCharacter.Instance.Binds.Character.Block.canceled -= OnBlockCancelled;
        _playerHitBox.DamageMultiplier = _defaultDamageMultiplier;
    }

    private void OnDisable()
    {
        PlayerCharacter.Instance.Binds.Character.Block.canceled -= OnBlockCancelled;
        _playerHitBox.TryParryAttack -= OnAttackTriedParry;
        _playerHitBox.DamageMultiplier = _defaultDamageMultiplier;
    }

    private void OnBlockCancelled(InputAction.CallbackContext obj)
    {
        CanChanged = true;
    }
}