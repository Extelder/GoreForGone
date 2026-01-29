using System.Collections;
using System.Collections.Generic;
using FishNet.Demo.AdditiveScenes;
using UnityEngine;

public interface IWeaponVisitor
{
    public void Visit(Projectile projectile);
    public void Visit(DesintegrationRing desintegrationRing, Vector3 hitpoint, Vector3 normal);
    public void Visit(PlayerDamageableAttack playerDamageableAttack, Vector3 hitPoint, Vector3 normal);
    public void Visit(SwordRaycastAttack SwordRaycastAttack, Vector3 hitPoint);
    public void Visit(MeleeTracer meleeTracer, Vector3 hitPoint);
}