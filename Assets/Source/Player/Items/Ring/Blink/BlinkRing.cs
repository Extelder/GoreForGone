using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlinkRing : PlayerRing
{
    [SerializeField] private float _offset;
    [SerializeField] private RaycastSettings _raycastSettings;
    [SerializeField] private GameObject _blinkToSpawn;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private GameObject _blinkGFX;

    private Vector3 _targetPosition;

    private void Start()
    {
        _blinkGFX = Instantiate(_blinkToSpawn, transform.position, Quaternion.identity);
    }

    protected override void OnRingAbilityBindStarted(InputAction.CallbackContext obj)
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (Physics.Raycast(_raycastSettings.Origin.position, _raycastSettings.Origin.forward,
                out RaycastHit hit, _raycastSettings.MaxDistance, _raycastSettings.LayerMask))
            {
                _targetPosition = hit.point + hit.normal * _offset;
            }
            else
            {
                _targetPosition = _raycastSettings.Origin.position +
                                  _raycastSettings.Origin.forward * _raycastSettings.MaxDistance;
                if (Physics.Raycast(_targetPosition, Vector3.down,
                    out RaycastHit hit2, 1, _raycastSettings.LayerMask))
                {
                    _targetPosition = hit2.point + hit2.normal * _offset;
                }
            }

            _blinkGFX.transform.position = _targetPosition;
        }).AddTo(_disposable);
        _blinkGFX.SetActive(true);
    }

    protected override void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
        PlayerCharacter.Instance.Teleport(_targetPosition);
        CancelAction();
        _blinkGFX.transform.position = Vector3.zero;
    }

    protected override void CancelAction()
    {
        _disposable?.Clear();
        _blinkGFX.SetActive(false);
    }
}