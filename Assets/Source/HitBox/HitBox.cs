using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public abstract class HitBox : NetworkBehaviour, IWeaponVisitor
{
    public abstract void Visit(Projectile projectile);

    public abstract void Visit(PlayerDamageableAttack playerDamageableAttack, Vector3 hitPoint, Vector3 normal);
    public abstract void Visit(PlayerDamageableAttack playerDamageableAttack, Vector3 hitPoint);

    [ServerRpc(RequireOwnership = false)]
    public void HitWithRaycast(Vector3 patriclePoint, Vector3 normal)
    {
        HitWithRaycastObsrever(patriclePoint, normal);
        OnHitWithRaycastServer(patriclePoint, normal);
    }

    public virtual void OnHitWithRaycastServer(Vector3 patriclePoint, Vector3 normal){}

    [ObserversRpc]
    public void HitWithRaycastObsrever(Vector3 patriclePoint, Vector3 normal){}
    
    public virtual void OnHitWithRaycastObserver(Vector3 patriclePoint, Vector3 normal){}

    [ServerRpc(RequireOwnership = false)]
    public void Hit(Vector3 overlapCenter, Vector3 patriclePoint)
    {
        HitObsrever(overlapCenter, patriclePoint);
        OnHitServer(overlapCenter, patriclePoint);
    }

    public virtual void OnHitServer(Vector3 overlapCenter, Vector3 particlePoint){}

    [ObserversRpc]
    public void HitObsrever(Vector3 overlapCenter, Vector3 patriclePoint)
    {
        OnHitObserver(overlapCenter, patriclePoint);
    }
    
    public virtual void OnHitObserver(Vector3 overlapCenter, Vector3 patriclePoint){}
}
