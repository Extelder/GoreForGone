using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class ObjectHitBox : HitBox
{
    public override void Visit(Projectile projectile)
    {
        Hit(transform, transform.position += new Vector3(0, 0.5f, 0));
    }

    public override void Visit(MeleeTracer meleeTracer)
    {
        Hit(transform, transform.position += new Vector3(0, 0.5f, 0));
    }

    public override void OnHitObserver(Transform overlapCenter, Vector3 patriclePoint)
    {
        base.OnHitObserver(overlapCenter, patriclePoint);
        Pools.Instance.ObjectHitPool.GetFreeElement(patriclePoint, Quaternion.identity);
    }

    public override void OnHitWithRaycastObserver(Vector3 patriclePoint, Vector3 normal)
    {
        base.OnHitWithRaycastObserver(patriclePoint, normal);
        Pools.Instance.ObjectHitPool.GetFreeElement(patriclePoint, Quaternion.LookRotation(normal));
    }
}
