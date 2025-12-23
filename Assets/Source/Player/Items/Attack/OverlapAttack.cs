using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapAttack : PlayerDamageableAttack
{
    [SerializeField] private Transform _attackOrigin;

    [SerializeField] private OverlapSettings _overlapSettings;

    public override event Action Performed;
    public override event Action StartAttack;

    public void PerformOverlapAttack()
    {
        StartAttack?.Invoke();
        Performed?.Invoke();

        _overlapSettings.Colliders = new Collider[_overlapSettings.Size];

        Physics.OverlapSphereNonAlloc(_overlapSettings.Origin.position, _overlapSettings.SphereRadius,
            _overlapSettings.Colliders, _overlapSettings.LayerMask);

        foreach (var other in _overlapSettings.Colliders)
        {
            if (other == null)
                continue;
            Vector3 hitPoint = other.ClosestPointOnBounds(_attackOrigin.position);
            Vector3 normal = (hitPoint - _overlapSettings.Origin.position).normalized;
            if (other.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor visitor))
            {
                visitor.Visit(this ,hitPoint, normal);
                return;
            }
            PlayerCharacter.Instance.ServerSpawnObject(PlayerCharacter.Instance.ParticlesHandler.ObjectHitParticle, hitPoint, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_overlapSettings.Origin.position, _overlapSettings.SphereRadius);
    }
}