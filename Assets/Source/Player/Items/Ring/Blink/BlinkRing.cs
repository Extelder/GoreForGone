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

    private void Start()
    {
        _blinkGFX = Instantiate(_blinkToSpawn, transform.position, Quaternion.identity);
    }

    protected override void OnRingAbilityBindStarted(InputAction.CallbackContext obj)
    {
        _blinkGFX.SetActive(true);
        Observable.EveryUpdate().Subscribe(_ =>
        {
            Vector3 targetPosition;
            if (Physics.Raycast(_raycastSettings.Origin.position, _raycastSettings.Origin.forward,
                out RaycastHit hit, _raycastSettings.MaxDistance, _raycastSettings.LayerMask))
            {
                targetPosition = hit.point + hit.normal * _offset;
            }
            else
            {
                targetPosition = _raycastSettings.Origin.position +
                                 _raycastSettings.Origin.forward * _raycastSettings.MaxDistance;
            }

            _blinkGFX.transform.position = targetPosition;
        }).AddTo(_disposable);
    }

    protected override void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
        CancelAction();
    }

    protected override void CancelAction()
    {
        _disposable?.Clear();
        _blinkGFX.SetActive(false);
    }
}