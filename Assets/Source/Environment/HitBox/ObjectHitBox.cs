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

    public override void Visit(MeleeTracer meleeTracer)
    {
        Hit(meleeTracer.BladeTip.position, transform.position);
    }

    public override void Visit(PlayerDamageableAttack playerDamageableAttackVector3, Vector3 hitPoint, Vector3 normal)
    {
        HitWithRaycast(hitPoint, normal);
        Debug.Log("RAYCAST");
    }

    public override void Visit(PlayerDamageableAttack playerDamageableAttack, Vector3 hitPoint)
    {
        Hit(hitPoint, hitPoint);
    }

    public override void OnHitObserver(Vector3 overlapCenter, Vector3 patriclePoint)
    {
        base.OnHitObserver(overlapCenter, patriclePoint);
        PlayerCharacter.Instance.ServerSpawnObject(PlayerCharacter.Instance.ParticlesHandler.ObjectHitParticle, patriclePoint, Quaternion.identity);
    }

    public override void OnHitWithRaycastObserver(Vector3 patriclePoint, Vector3 normal)
    {
        base.OnHitWithRaycastObserver(patriclePoint, normal);
        PlayerCharacter.Instance.ServerSpawnObject(PlayerCharacter.Instance.ParticlesHandler.ObjectHitParticle, patriclePoint, Quaternion.LookRotation(normal));
    }
}
