using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SwordAttackState : SwordState
{
    [SerializeField] private float _continueAttackCheckRate;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private bool _conntinueAttacking = false;

    public override void Enter()
    {
        CanChanged = false;
        Animator.Attack();
    }

    public void StartCheckingForContinueAttacking()
    {
        _disposable.Clear();
        _conntinueAttacking = false;
        Observable.Interval(TimeSpan.FromSeconds(_continueAttackCheckRate)
        ).Subscribe(_ =>
        {
            if (PlayerCharacter.Instance.Binds.Character.MainShoot.IsPressed())
            {
                _conntinueAttacking = true;
                _disposable.Clear();
            }
        }).AddTo(_disposable);
    }

    public override void Exit()
    {
        _disposable?.Clear();
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }

    public void StopCheckingForContinueAttacking()
    {
        _disposable.Clear();
    }

    public void AttackAnimationEnd()
    {
        CanChanged = !_conntinueAttacking;
        if (CanChanged == false)
        {
            Animator.RandomizeAtackAnimation();
        }
    }
}