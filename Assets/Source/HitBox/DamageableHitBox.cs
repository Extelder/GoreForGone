using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public abstract class DamageableHitBox : HitBox
{
    [field: SerializeField] public EnemyHealth EnemyHealth { get; private set; }
    
    public abstract override void Visit(Projectile projectile);
    public abstract override void Visit(MeleeTracer meleeTracer);
    
    [ServerRpc(RequireOwnership = false)]
    public void HitWithRaycast(float damage, Vector3 patriclePoint, Vector3 normal)
    {
        HitWithRaycastObsrever(damage, patriclePoint, normal);
        OnHitWithRaycastServer(patriclePoint, normal);
    }

    public virtual void OnHitWithRaycastServer(Vector3 patriclePoint, Vector3 normal){}

    [ObserversRpc]
    public void HitWithRaycastObsrever(float damage, Vector3 patriclePoint, Vector3 normal)
    {
        OnHitWithRaycastObserver(damage, patriclePoint, normal);
    }

    public virtual void OnHitWithRaycastObserver(float damage, Vector3 patriclePoint, Vector3 normal)
    {
        //Pools.Instance.BloodPool.GetFreeElement(patriclePoint, Quaternion.LookRotation(normal));
        EnemyHealth.TakeDamage(damage);
    }

    [ServerRpc(RequireOwnership = false)]
    public void Hit(Transform overlapCenter, float damage, Vector3 patriclePoint)
    {
        HitObsrever(overlapCenter, damage, patriclePoint);
        OnHitServer(overlapCenter);
    }

    public virtual void OnHitServer(Transform overlapCenter){}

    [ObserversRpc]
    public void HitObsrever(Transform overlapCenter, float damage, Vector3 patriclePoint)
    {
        OnHitObserver(overlapCenter, damage, patriclePoint);
    }

    public virtual void OnHitObserver(Transform overlapCenter, float damage, Vector3 patriclePoint)
    {
        //Pools.Instance.BloodPool.GetFreeElement(patriclePoint, Quaternion.identity);
        EnemyHealth.TakeDamage(damage);
    }
}
