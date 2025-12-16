using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.Mathematics;
using UnityEngine;

public class UnitHitBox : DamageableHitBox
{
    [field: SerializeField] public EnemyHealth EnemyHealth;

    public override void Visit(Projectile projectile)
    {
        Hit(transform, projectile.Damage,
            transform.position += new Vector3(0, 0.5f, 0));
    }

    public override void Visit(MeleeTracer meleeTracer)
    {
        Hit(transform, meleeTracer.Damage,
            transform.position += new Vector3(0, 0.5f, 0));
    }
}