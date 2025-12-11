using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapAttack : PlayerAttack
{
    [SerializeField] private OverlapSettings _overlapSettings;

    [SerializeField] private GameObject _testAttackShit;

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
            Vector3 hitPoint = other.ClosestPoint(PlayerCharacter.Instance.Camera.position);
            Vector3 normal = (hitPoint - _overlapSettings.Origin.position).normalized;
            Instantiate(_testAttackShit, hitPoint, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_overlapSettings.Origin.position, _overlapSettings.SphereRadius);
    }
}