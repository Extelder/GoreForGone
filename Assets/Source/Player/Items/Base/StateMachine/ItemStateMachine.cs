using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class ItemStateMachine : StateMachine
{
    [SerializeField] private PlayerCharacter _character;
    [SerializeField] private ItemState _idleState;
    [SerializeField] private ItemState _moveState;

    private CompositeDisposable _disposable = new CompositeDisposable();

    protected PlayerCharacter playerCharacter;

    public abstract void OnInitializeted();

    private void Start()
    {
        Init();
        playerCharacter = _character;
        playerCharacter.ClientStarted += OnPlayerStarted;
        OnInitializeted();
    }

    protected virtual void OnPlayerStarted()
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (playerCharacter.PlayerController.Moving.Value)
            {
                Move();
                return;
            }

            Idle();
        }).AddTo(_disposable);
    }

    protected virtual void OnDisableVirtual()
    {
    }

    private void OnDisable()
    {
        _disposable.Clear();
        playerCharacter.ClientStarted -= OnPlayerStarted;
        OnDisableVirtual();
    }

    public void Idle()
    {
        ChangeState(_idleState);
    }

    public void Move()
    {
        ChangeState(_moveState);
    }
}