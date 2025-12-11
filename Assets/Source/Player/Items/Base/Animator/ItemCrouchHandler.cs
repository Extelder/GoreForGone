using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemCrouchHandler : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _crouchAnimationBoolName;

    [SerializeField] private PlayerCharacter _character;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        _character.ClientStarted += OnClientStarted;
    }

    private void OnClientStarted()
    {
        _character.PlayerController.isCrough.Subscribe(_ => { _animator.SetBool(_crouchAnimationBoolName, _); })
            .AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable?.Clear();
        _character.ClientStarted -= OnClientStarted;
    }
}