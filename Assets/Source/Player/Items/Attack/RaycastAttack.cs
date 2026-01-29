using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAttack : PlayerDamageableAttack
{
    [SerializeField] private RaycastSettings _raycastSettings;

    public override event Action Performed;
    public override event Action StartAttack;

    public event Action Hitted;

    private RaycastHit _hit;

    public bool HittedNonDamagableObject()
    {
        if (Physics.Raycast(_raycastSettings.Origin.position, _raycastSettings.Origin.forward, out _hit,
            _raycastSettings.MaxDistance, _raycastSettings.LayerMask))
        {
            return true;
        }

        return false;
    }

    public void PerformRaycastAttack()
    {
        StartAttack?.Invoke();
        Performed?.Invoke();

        if (Physics.Raycast(_raycastSettings.Origin.position, _raycastSettings.Origin.forward, out _hit,
            _raycastSettings.MaxDistance, _raycastSettings.LayerMask))
        {
            if (_hit.collider.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor visitor))
            {
                Accept(visitor, _hit.point);
                Hitted?.Invoke();
                return;
            }

            PlayerCharacter.Instance.ServerSpawnObject(PlayerCharacter.Instance.ParticlesHandler.ObjectHitParticle,
                _hit.point, Quaternion.LookRotation(_hit.normal));
            Hitted?.Invoke();
        }
    }

    public virtual void Accept(IWeaponVisitor visitor, Vector3 point)
    {
    }
}