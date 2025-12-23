using System.Collections;
using System.Collections.Generic;
using FishNet.Demo.AdditiveScenes;
using UnityEngine;

public interface IWeaponVisitor
{
    public void Visit(Projectile projectile);
    public void Visit(MeleeTracer meleeTracer);
    public void Visit(PlayerDamageableAttack playerDamageableAttack, Vector3 hitPoint, Vector3 normal);
    public void Visit(PlayerDamageableAttack playerDamageableAttack, Vector3 hitPoint);
}