using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class HitBox : NetworkBehaviour, IWeaponVisitor
{
    public virtual void Visit(Projectile projectile)
    {
    }
}
