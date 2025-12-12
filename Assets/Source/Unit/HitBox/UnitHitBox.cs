using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.Mathematics;
using UnityEngine;

public class UnitHitBox : HitBox
{
    [field :SerializeField] public EnemyHealth EnemyHealth;

    [ServerRpc(RequireOwnership = false)]
    public void HitWithRaycast(float damage, Vector3 point, Vector3 normal)
    {
        HitWithRaycastObsrever(damage, point, normal);
        OnHitWithRaycastServer(point, normal);
    }

    public virtual void OnHitWithRaycastServer(Vector3 point, Vector3 normal){}

    [ObserversRpc]
    public void HitWithRaycastObsrever(float damage, Vector3 point, Vector3 normal)
    {
        EnemyHealth.TakeDamage(damage);
        Pools.Instance.BloodPool.GetFreeElement(point, Quaternion.LookRotation(normal));
    }

    [ServerRpc(RequireOwnership = false)]
    public void Hit(Transform overlapCenter, float damage, Vector3 bloodPoint)
    {
        HitObsrever(overlapCenter ,damage, bloodPoint);
        OnHitServer(overlapCenter);
    }
    
    public virtual void OnHitServer(Transform overlapCenter){}

    [ObserversRpc]
    public void HitObsrever(Transform overlapCenter, float damage, Vector3 bloodPoint)
    {
        EnemyHealth.TakeDamage(damage);
        Pools.Instance.BloodPool.GetFreeElement(bloodPoint, quaternion.identity);
    }
}
