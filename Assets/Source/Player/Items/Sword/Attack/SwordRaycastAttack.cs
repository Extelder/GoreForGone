using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordRaycastAttack : RaycastAttack
{
    public override void Accept(IWeaponVisitor visitor, Vector3 point)
    {
        visitor.Visit(this, point);
    }
}