using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class ObjectHitBox : HitBox
{
    public override void Visit(Projectile projectile)
    {
        Hit(transform.position, transform.position);
    }

    public override void Visit(PlayerDamageableAttack playerDamageableAttackVector3, Vector3 hitPoint, Vector3 normal)
    {
        HitWithRaycast(hitPoint, normal);
    }

    public override void Visit(PlayerDamageableAttack playerDamageableAttack, Vector3 hitPoint)
    {
        Hit(hitPoint, hitPoint);
    }
}
