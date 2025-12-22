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
    public bool CanExplode { get; private set; } = true;
    private void OnCollisionEnter(Collision other)
    {
        OnCollisionEnterVirtual(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterVirtual(other);
    }

    public virtual void OnCollisionEnterVirtual(Collision other)
    {
        if (!CanExplode)
            return;
        Explode();   
    }

    public virtual void OnTriggerEnterVirtual(Collider other)
    {
        if (!CanExplode)
            return;
        Explode();   
    }

    public void Explode()
    {
        CanExplode = false;
        _colliders = new Collider[_overlapSettings.Size];
        Overlap();
        foreach (var other in _colliders)
        {
            if (other == null)
                continue;
            CheckOnComponents(other);
        }

        ServerDespawn();
    }

    public virtual void CheckOnComponents(Collider other)
    {
        if (other.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor visitor))
        {
            visitor.Visit(this);
        }
        if (other.TryGetComponent<PlayerHitBox>(out PlayerHitBox playerHitBox))
        {
            playerHitBox.TakeDamage(Damage);
        }
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
        CanExplode = true;
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

    private void OnDisable()
    {
        if (!base.IsOwner)
            return;
        CanExplode = false;
    }
}
