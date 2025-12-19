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
            transform.position += new Vector3(0, 0.5f, 0));
    }

    public override void Visit(MeleeTracer meleeTracer)
    {
        Hit(meleeTracer.BladeTip.position, meleeTracer.Damage,
            transform.position += new Vector3(0, 0.5f, 0));
    }
}