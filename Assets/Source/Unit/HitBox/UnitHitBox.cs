using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.Mathematics;
using UnityEngine;

public class UnitHitBox : DamageableHitBox
{
    public override void Visit(Projectile projectile)
    {
        Hit(transform.position, projectile.Damage,
            transform.position);
    }

    public override void Visit(DesintegrationRing desintegrationRing, Vector3 hitPoint, Vector3 normal)
    {
        HitWithRaycast(desintegrationRing.Damage, hitPoint, normal);
    }

    public override void Visit(PlayerDamageableAttack playerDamageableAttack, Vector3 hitPoint, Vector3 normal)
    {
        HitWithRaycast(playerDamageableAttack.Damage, hitPoint, 
            transform.position += new Vector3(0.5f, 0, 0));
    }

    public override void Visit(PlayerDamageableAttack playerDamageableAttack, Vector3 hitPoint)
    {
        Hit(hitPoint, playerDamageableAttack.Damage,
            transform.position += new Vector3(0.5f, 0, 0));
    }
}