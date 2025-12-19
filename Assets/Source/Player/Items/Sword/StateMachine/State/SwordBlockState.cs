using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordBlockState : SwordState
{
    [SerializeField] private PlayerHitBox _playerHitBox;
    [SerializeField] private float _blockDamageMultiplier;
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
        if (_succesfullyParried)
        {
            Debug.Log("SUCCESFULLY PARRIED");
            _playerHitBox.DamageMultiplier = 0;
            _parriableEnemyStateMachine.React();   
            return;
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