using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponVisitor
{
    public void Visit(Projectile projectile);
    public void Visit(MeleeTracer meleeTracer);
}