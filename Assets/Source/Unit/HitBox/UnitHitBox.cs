using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.Mathematics;
using UnityEngine;

public class UnitHitBox : HitBox
{
    [SerializeField] private EnemyHealth _enemyHealth;
    
    [ServerRpc(RequireOwnership = false)]
    public void HitWithRaycast(float damage, Vector3 point, Vector3 normal)
    {
        HitWithRaycastObsrever(damage, point, normal);
    }

    [ObserversRpc]
    public void HitWithRaycastObsrever(float damage, Vector3 point, Vector3 normal)
    {
        _enemyHealth.TakeDamage(damage);
        Pools.Instance.BloodPool.GetFreeElement(point, Quaternion.LookRotation(normal));
    }

    [ServerRpc(RequireOwnership = false)]
    public void Hit(float damage, Vector3 bloodPoint)
    {
        HitObsrever(damage, bloodPoint);
    }

    [ObserversRpc]
    public void HitObsrever(float damage, Vector3 bloodPoint)
    {
        _enemyHealth.TakeDamage(damage);
        Pools.Instance.BloodPool.GetFreeElement(bloodPoint, quaternion.identity);
    }
}
