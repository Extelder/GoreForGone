using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

public abstract class ItemStateMachine : StateMachine
{
    [SerializeField] private bool _canInspect;
    [SerializeField] private PlayerCharacter _character;
    [SerializeField] private ItemState _idleState;
    [SerializeField] private ItemState _moveState;

    [ShowIf(nameof(_canInspect))] [SerializeField]
    private ItemState _inspectState;


    private CompositeDisposable _disposable = new CompositeDisposable();

    protected PlayerCharacter playerCharacter;

    public abstract void OnInitializeted();

    private void Awake()
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
        if (_canInspect)
        {
        }
    }

    protected virtual void OnDisableVirtual()
    {
    }

    private void OnDisable()
    {
        if (!base.IsOwner)
            return;
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