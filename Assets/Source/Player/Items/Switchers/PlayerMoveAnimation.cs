using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerMoveAnimation : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _character;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _moveBoolName = "IsMoving";

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Start()
    {
        _character.PlayerController.Moving.Subscribe(_ => { _animator.SetBool(_moveBoolName, _); })
            .AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}