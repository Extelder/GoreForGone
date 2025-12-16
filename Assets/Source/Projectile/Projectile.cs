using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public abstract class Projectile : NetworkBehaviour
{
    [field :SerializeField] public float Damage { get; private set; }
    [SerializeField] private OverlapSettings _overlapSettings;
    [SerializeField] private float _cooldowmToDespawn;
    private Collider[] _colliders;
    private bool _canExplode = true;
    private void OnCollisionEnter(Collision other)
    {
        if (!_canExplode)
            return;
        Explode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_canExplode)
            return;
        Explode();
    }

    private void Explode()
    {
        _colliders = new Collider[_overlapSettings.Size];
        Overlap();
        foreach (var other in _colliders)
        {
            if (other == null)
                continue;
            if (other.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor visitor))
            {
                visitor.Visit(this);
            }
            if (other.TryGetComponent<PlayerHitBox>(out PlayerHitBox playerHitBox))
            {
                playerHitBox.TakeDamage(Damage);
            }
        }

        ServerDespawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ServerDespawn()
    {
        ObseverDespawn();
    }

    [ObserversRpc]
    public void ObseverDespawn()
    {
        StartCoroutine(Despawning());
    }

    private IEnumerator Despawning()
    {
        yield return new WaitForSeconds(_cooldowmToDespawn);
        _canExplode = true;
        Despawn();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_overlapSettings.Origin.position,
            _overlapSettings.SphereRadius);
    }

    private void Overlap()
    {
        _overlapSettings.Size = Physics.OverlapSphereNonAlloc(_overlapSettings.Origin.position,
            _overlapSettings.SphereRadius, _colliders, _overlapSettings.LayerMask);
    }
}
