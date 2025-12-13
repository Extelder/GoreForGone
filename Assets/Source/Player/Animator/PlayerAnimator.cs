using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FishNet.Object;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed;

    [SerializeField] private PlayerCharacter _character;

    private PlayerBinds _binds;

    private Vector3 _inputVector;
    private Vector3 _finalVector;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        _character.ClientStarted += OnClienStarted;
    }

    private void OnClienStarted()
    {
        if (!_character.IsOwner)
            return;

        _binds = _character.Binds;

        _binds.Character.Crouch.started += OnCrouchStarted;
        _binds.Character.Crouch.canceled += OnCrouchCanceled;

        Observable.EveryUpdate().Subscribe(_ =>
        {
            _inputVector = new Vector3(_binds.Character.Horizontal.ReadValue<float>(), 0,
                _binds.Character.Vertical.ReadValue<float>());

            _finalVector.x = Mathf.Lerp(_finalVector.x, _inputVector.x, _speed * Time.deltaTime);
            _finalVector.z = Mathf.Lerp(_finalVector.z, _inputVector.z, _speed * Time.deltaTime);

            SetAnimationBool("Move", Mathf.Abs(_inputVector.x) > 0 || Mathf.Abs(_inputVector.z) > 0);
            _animator.SetFloat("XVelocity", _finalVector.x);
            _animator.SetFloat("YVelocity", _finalVector.z);
        }).AddTo(_disposable);
    }

    private void OnCrouchCanceled(InputAction.CallbackContext obj)
    {
        SetAnimationBool("Crouch", false);
    }

    private void OnCrouchStarted(InputAction.CallbackContext obj)
    {
        SetAnimationBool("Crouch", true);
    }

    public void SetAnimationBoolAndDisableOthers(string name, bool value)
    {
        DisableAll();
        _animator.SetBool(name, value);
    }

    public void SetAnimationBool(string name, bool value)
    {
        _animator.SetBool(name, value);
    }

    public void SetAnimationTrigger(string name)
    {
        _animator.SetTrigger(name);
    }

    public void SetAnimationTriggerAndDisableOthers(string name)
    {
        DisableAll();
        _animator.SetTrigger(name);
    }

    public void ResetAnimationTrigger(string name)
    {
        _animator.ResetTrigger(name);
    }

    public void Equip(int id)
    {
        EquipServer(id);
    }

    [ServerRpc(RequireOwnership = false)]
    public void EquipServer(int id)
    {
        EquipObserver(id);
    }

    [ObserversRpc]
    public void EquipObserver(int id)
    {
        _animator.SetInteger("EquipId", id);
        _animator.SetTrigger("Equip");
    }

    public void DisableAll()
    {
    }

    public void SetLocomotionBlendTreeSpeed(float speed)
    {
        _animator.SetFloat("BlendTreeSpeed", speed);
    }

    private void OnDestroy()
    {
        if (_character.IsOwner)
        {
            _binds.Character.Crouch.canceled -= OnCrouchCanceled;
            _binds.Character.Crouch.started -= OnCrouchStarted;
        }

        _character.ClientStarted -= OnClienStarted;
        _disposable?.Clear();
    }
}