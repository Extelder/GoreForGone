using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Demo.AdditiveScenes;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesintegrationRing : PlayerRing
{
    [field: SerializeField] public float Damage { get; private set; }
    [SerializeField] private RaycastSettings _raycastSettings;
    [SerializeField] private GameObject _spawnObject;
    [SerializeField] private int _maxOffsetCount;
    [SerializeField] private float _offset;

    protected override void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
        if (!base.IsOwner)
            return;
        PlayerCharacter.Instance.ServerSpawnObject(_spawnObject, _raycastSettings.Origin.position, _raycastSettings.Origin.rotation);
        for (int i = 0; i < _maxOffsetCount; i++)
        {
            Debug.DrawRay(_raycastSettings.Origin.position,
                (_raycastSettings.Origin.forward + _raycastSettings.Origin.rotation * new Vector3(_offset * i, 0, 0)) * _raycastSettings.MaxDistance,
                Color.red, 2);
            if (Physics.Raycast(_raycastSettings.Origin.position,
                _raycastSettings.Origin.forward + _raycastSettings.Origin.rotation * new Vector3(_offset * i, 0, 0), out RaycastHit hitRight,
                _raycastSettings.MaxDistance, _raycastSettings.LayerMask))
            {
                if (hitRight.collider.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor visitor))
                {
                    visitor.Visit(this, hitRight.point, hitRight.normal);
                }
            }

            Debug.DrawRay(_raycastSettings.Origin.position,
                (_raycastSettings.Origin.forward + _raycastSettings.Origin.rotation * new Vector3(-_offset * i, 0, 0)) * _raycastSettings.MaxDistance,
                Color.blue, 2);
            if (Physics.Raycast(_raycastSettings.Origin.position,
                _raycastSettings.Origin.forward + _raycastSettings.Origin.rotation * new Vector3(-_offset * i, 0, 0), out RaycastHit hitLeft,
                _raycastSettings.MaxDistance, _raycastSettings.LayerMask))
            {
                if (hitLeft.collider.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor visitor))
                {
                    visitor.Visit(this, hitLeft.point, hitLeft.normal);
                }
            }
        }
    }

    protected override void CancelAction()
    {
        if (!base.IsOwner)
            return;
    }
}