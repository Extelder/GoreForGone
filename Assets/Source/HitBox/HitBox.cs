using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public abstract class HitBox : NetworkBehaviour, IWeaponVisitor
{
    public abstract void Visit(Projectile projectile);

    public abstract void Visit(MeleeTracer meleeTracer);
    
    
    [ServerRpc(RequireOwnership = false)]
    public virtual void HitWithRaycast(Vector3 patriclePoint, Vector3 normal)
    {
        HitWithRaycastObsrever(patriclePoint, normal);
        OnHitWithRaycastServer(patriclePoint, normal);
    }

    public virtual void OnHitWithRaycastServer(Vector3 patriclePoint, Vector3 normal){}

    [ObserversRpc]
    public virtual void HitWithRaycastObsrever(Vector3 patriclePoint, Vector3 normal){}
    
    public virtual void OnHitWithRaycastObserver(Vector3 patriclePoint, Vector3 normal){}

    [ServerRpc(RequireOwnership = false)]
    public virtual void Hit(Transform overlapCenter, Vector3 patriclePoint)
    {
        HitObsrever(overlapCenter, patriclePoint);
        OnHitServer(overlapCenter, patriclePoint);
    }

    public virtual void OnHitServer(Transform overlapCenter, Vector3 particlePoint){}

    [ObserversRpc]
    public virtual void HitObsrever(Transform overlapCenter, Vector3 patriclePoint)
    {
        OnHitObserver(overlapCenter, patriclePoint);
    }
    
    public virtual void OnHitObserver(Transform overlapCenter, Vector3 patriclePoint){}
}
