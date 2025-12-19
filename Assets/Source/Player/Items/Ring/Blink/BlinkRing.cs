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

    public override void OnStartClient()
    {
        if (!base.IsOwner)
            return;
        _blinkGFX = Instantiate(_blinkToSpawn, transform.position, Quaternion.identity);
        PlayerCharacter.Instance.ServerManager.Spawn(_blinkGFX);
    }

    protected override void OnRingAbilityBindStarted(InputAction.CallbackContext obj)
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (Physics.Raycast(_raycastSettings.Origin.position, _raycastSettings.Origin.forward,
                out RaycastHit hit, _raycastSettings.MaxDistance, _raycastSettings.LayerMask))
            {
                _targetPosition = hit.point + hit.normal * _offset;

                Collider[] other = new Collider[20];
                Physics.OverlapCapsuleNonAlloc(_targetPosition - new Vector3(0, 0.3f, 0),
                    _targetPosition + new Vector3(0, 0.3f, 0), 0.4f, other);
                for (int i = 0; i < other.Length; i++)
                {
                    if (other[i] != null)
                    {
                        Debug.Log(other[i].name);
                        return;
                    }
                }
            }
            else
            {
                _targetPosition = _raycastSettings.Origin.position +
                                  _raycastSettings.Origin.forward * _raycastSettings.MaxDistance;
                if (Physics.Raycast(_targetPosition, Vector3.down,
                    out RaycastHit hit2, 1, _raycastSettings.LayerMask))
                {
                    _targetPosition = hit2.point + hit2.normal * _offset;

                    Collider[] other = new Collider[20];
                    Physics.OverlapCapsuleNonAlloc(_targetPosition - new Vector3(0, 0.3f, 0),
                        _targetPosition + new Vector3(0, 0.3f, 0), 0.4f, other);
                    for (int i = 0; i < other.Length; i++)
                    {
                        if (other[i] != null)
                        {
                            Debug.Log(other[i].name);
                            return;
                        }
                    }
                }
            }

            _blinkGFX.transform.position = _targetPosition;
        }).AddTo(_disposable);

        PlayerCharacter.Instance.SetObjectEnableServer(_blinkGFX, true);
        _blinkGFX.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        if (_blinkGFX == null)
            return;
        Gizmos.DrawWireSphere(_blinkGFX.transform.position - new Vector3(0, 0.5f, 0), .4f);
        Gizmos.DrawWireSphere(_blinkGFX.transform.position + new Vector3(0, 0.5f, 0), 0.4f);
    }

    protected override void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
        if (!_blinkGFX.activeInHierarchy)
        {
            _blinkGFX.transform.position = Vector3.zero;
            CancelAction();
            return;
        }

        PlayerCharacter.Instance.Teleport(_targetPosition);
        CancelAction();
    }

    protected override void CancelAction()
    {
        if (!base.IsOwner)
            return;
        _disposable?.Clear();

        PlayerCharacter.Instance.SetObjectEnableServer(_blinkGFX, false);
        _blinkGFX.SetActive(false);
    }
}